using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarGame.Vehicle
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
        public FixedJoint Joint;
        public bool WasPlaced = false;
        List<int> _proxiesLayersMemory;
        private Rigidbody _rigidbody;
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
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.sleepThreshold = -1.0f;
            partData = _partScriptable?.data;
            Proxies = GetComponentsInChildren<PartProxy>().ToList();
            //An additional save to avoid null reference exceptions (just in case)
            SaveProxiesLayers();
        }
        public void SaveProxiesLayers() => _proxiesLayersMemory = Proxies.Select(_ => _.gameObject.layer).ToList();
        public void RestoreProxiesLayers()
        {
            for (int i = 0; i < Proxies.Count; i++)
                Proxies[i].gameObject.layer = _proxiesLayersMemory[i];
        }
        public void SetProxiesLayer(int layer, bool save = false)
        {
            if (save)
                SaveProxiesLayers();
            Proxies.ForEach(_ => _.gameObject.layer = layer);
        }
    }
}