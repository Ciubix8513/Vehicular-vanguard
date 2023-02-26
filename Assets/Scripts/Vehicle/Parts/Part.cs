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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VehicularVanguard.Vehicle
{
    public struct ActionData
    {
        public Part.ActionDel Delegate;
        public string Name;
        public KeyCode Key;
        public int ActionType;
        public ActionData(Part.ActionDel @delegate, string name, KeyCode key, int actionType)
        {
            Delegate = @delegate;
            Name = name;
            Key = key;
            ActionType = actionType;
        }
    }
    public class Part : MonoBehaviour
    {
        [SerializeField]
        private PartScriptable _partScriptable;
        public PartData partData;
        // public int Id;
        public int health;
        public Vector3 m_size;
        public Attachments attachedParts;
        public Vector3Int parentFace;
        public Part parentPart;
        public bool isRoot;
        public bool Activatable;
        public bool isActive;
        public Dictionary<string, Tuple<KeyCode, int>> binds = new();
        public List<PartProxy> Proxies;
        public Joint Joint;
        public bool WasPlaced = false;
        int _layerMemory;
        List<int> _proxiesLayersMemory = new();
        public Rigidbody Rigidbody;
        public GameObject DraggingObject;
        public void TakeDamage(int dmg) => health -= dmg;
        public delegate void ActionDel();
        public virtual List<ActionData> GetActions() =>
        new() {
             new(Activate, "Activate", KeyCode.W, 0),
             new(DeActivate, "Deactivate", KeyCode.W, 1)};
        public virtual void Activate() => isActive = true;
        public virtual void DeActivate() => isActive = false;
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.sleepThreshold = -1.0f;
            partData = _partScriptable?.data;
            Proxies = GetComponentsInChildren<PartProxy>().ToList();
            if (DraggingObject == null)
                DraggingObject = gameObject;
            //An additional save to avoid null reference exceptions (just in case)
            SaveProxiesLayers();
        }
        public void SaveProxiesLayers()
        {
            _proxiesLayersMemory = Proxies.Select(_ => _.gameObject.layer).ToList();
            _layerMemory = gameObject.layer;
        }

        public void RestoreProxiesLayers(bool restoreSelf = false)
        {
            for (int i = 0; i < Proxies.Count; i++)
                if (Proxies[i] != null)
                    Proxies[i].gameObject.layer = _proxiesLayersMemory[i];
            if (restoreSelf)
                gameObject.layer = _layerMemory;
        }
        public void SetProxiesLayer(int layer, bool save = false, bool changeSelf = false)
        {
            if (save)
                SaveProxiesLayers();
            if (changeSelf)
                gameObject.layer = layer;
            Proxies.ForEach(action: _ => _.gameObject.layer = layer);
            Debug.Log($"Setting layers of the proxies of {gameObject.name} to {layer}");
        }
        //Function to connect parts, virtual for things like wheels 
        public virtual void PartConnect(Part other)
        {
            Joint = gameObject.AddComponent<FixedJoint>();
            Joint.connectedBody = other.Rigidbody;
        }
    }
}