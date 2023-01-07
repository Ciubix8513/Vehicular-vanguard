using UnityEngine;
using CarGame.Vehicle.Saving;
using System.Collections.Generic;
using System.Linq;
using CarGame.UI;
using CarGame.Vehicle.Editor.UI;
using CarGame.Player;

namespace CarGame.Vehicle.Editor
{
    public class HistoryManager : MonoBehaviour
    {
        public delegate void HistoryHandler();
        public static event HistoryHandler HistoryChangedEvent;
        private static List<HistoryVehicle> s_history;
        //Just for debugging
        [SerializeField]
        private static Transform s_vehicleRoot;
        public static Part Root;
        //Index to current action
        private static int s_action_index_private;
        public static int HistoryLength
        {
            get
            {
                if (s_history != null)
                    return s_history.Count;
                return 0;
            }
        }

        public static int ActionIndex
        {
            get => s_action_index_private;
            set
            {
                if (value < 0) return;
                if (value == s_action_index_private) return;
                //Undo
                if (value < ActionIndex)
                    if (value < s_history.Count - 1)
                    {
                        s_action_index_private = value;
                        Generate(ActionIndex);
                        return;
                    }
                    else return;
                //Redo
                if (value > s_history.Count - 1) return;
                s_action_index_private = value;
                Generate(ActionIndex);
            }
        }
        static void Generate(int index)
        {
            print("Trying to generate for " + index + " length = " + s_history.Count);
            var res = VehicleSaver.GenerateHistoryVehicle(s_history[index], s_vehicleRoot);
            Root = res.Item1;
            InputManager.SetGameCameraTarget(Root.transform.parent);
            HistoryChangedEvent();
            if (s_history[index].EditorMode == EditorMode.input)
            {
                print("Loading history vehicle in input mode");
                UIManager.ActivateTab(1);
                InputMenu.s_this.OnMenuOpen();
                InputMenu.s_this.LoadData(res.Item2, true);
            }
            else if(s_history[index].EditorMode == EditorMode.rotate && InputManager.editorMode == EditorMode.rotate)
            {
                UIManager.ActivateTab(0);
                GizmoRaycaster.SetRotatingObject(res.Item2.gameObject);
            }
        }

        void Awake()
        {
            s_history = new();
            s_action_index_private = -1;
        }
        void Start()
        {
            Root = FindObjectsOfType<Part>().Where(_ => _.isRoot).FirstOrDefault();
            s_vehicleRoot = Root.transform.parent;
            //Save the initial state 
            ProcessChange("Initial save");
        }
        //A function that saves the state of a vehicle
        public static void ProcessChange(string e)
        {
            print("Processing history changed caused by " + e);
            //If we're in the past clear all actions ahead
            if (ActionIndex < s_history.Count - 1)
                s_history.RemoveRange(ActionIndex + 1, s_history.Count - (ActionIndex + 1));
            s_history.Add(VehicleSaver.SerializeHistoryVehicle(Root));
            s_action_index_private++;
            HistoryChangedEvent();
        }
        public static void ResetHistory()
        {
            s_action_index_private = -1;
            s_history.Clear();
            ProcessChange("History reset");
        }
    }
}