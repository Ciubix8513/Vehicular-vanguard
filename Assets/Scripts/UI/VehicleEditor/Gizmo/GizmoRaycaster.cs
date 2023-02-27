//The GPLv3 License (GPLv3)
//
//Copyright (c) 2023 Ciubix8513
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
using VehicularVanguard.Player;
using UnityEngine;

namespace VehicularVanguard.Vehicle.Editor
{
    public class GizmoRaycaster : MonoBehaviour
    {
        enum Axis
        {
            x, y, z
        }
        public static bool Rotating = false;
        private static GameObject s_rotation;
        private static Camera s_camera;
        [SerializeField]
        private GameObject _rotationPrefab;
        private static Vector3 s_axis;
        private static Part s_rotatingObj;
        public static int RotatingObjectId
        {
            get
            {
                if (s_rotatingObj != null)
                    return s_rotatingObj.GetHashCode();
                return 0;
            }
        }
        private void Awake()
        {
            s_camera = Camera.main;
            s_rotation = Instantiate(_rotationPrefab);
            s_rotation.SetActive(false);
        }
        public static void HideGizmo() => s_rotation.SetActive(false);
        public void SwitchMode(int mode)
        {
            Rotating = false;
            if (s_rotation)
                s_rotatingObj?.RestoreProxiesLayers();
            switch (mode)
            {
                case 1:
                    InputManager.editorMode = EditorMode.rotate;
                    break;
                case 2:
                    InputManager.editorMode = EditorMode.input;
                    s_rotation.SetActive(false);
                    break;
                default:
                    InputManager.editorMode = EditorMode.place;
                    s_rotation.SetActive(false);
                    break;
            }
        }
        public static void Raycast()
        {
            if (InputManager.editorMode != EditorMode.rotate) return;
            if (Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition), out var hit, 500.0f, 1 << 8))
            {
                if (!hit.collider.CompareTag("Gizmo")) goto ray2;
                Rotating = true;
                if (hit.collider.name.ToLower() == "x")
                    s_axis = Vector3.right;
                else if (hit.collider.name.ToLower() == "y")
                    s_axis = Vector3.up;
                else
                    s_axis = Vector3.forward;
                Rotate(s_axis);
            }
        ray2:
            if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition), out hit, 500.0f)) return;
            if (hit.collider.CompareTag("Part"))
            {
                SetRotatingObject(hit.collider.gameObject);
                return;
            }
        }
        public static void SetRotatingObject(GameObject o)
        {
            if (s_rotatingObj != null)
                s_rotatingObj?.RestoreProxiesLayers();
            s_rotatingObj = o.GetComponent<PartProxy>().part;
            s_rotatingObj.SaveProxiesLayers();
            s_rotatingObj.SetProxiesLayer(2);
            s_rotation.transform.position = o.transform.position;
            // s_rotation.transform.rotation = o.transform.rotation;
            s_rotation.SetActive(true);
        }
        static void Rotate(Vector3 axis)
        {
            var parts = s_rotatingObj.attachedParts;
            parts[s_rotatingObj.parentFace] = false;

            //THANK YOU CHATGPT
            s_rotatingObj.transform.rotation = 
            Quaternion.AngleAxis(Input.GetKey(KeyCode.LeftShift) ? -90 : 90, axis) 
            * s_rotatingObj.transform.rotation;
            //This is a pretty dumb fix but it should work
            var body = s_rotatingObj.Joint.connectedBody;
            Destroy(s_rotatingObj.Joint);
            (s_rotatingObj.Joint = s_rotatingObj.gameObject.AddComponent<FixedJoint>()).connectedBody = body;
            VehicularVanguard.Vehicle.Editor.HistoryManager.ProcessChange("Rotating a part");
        }
    }
}