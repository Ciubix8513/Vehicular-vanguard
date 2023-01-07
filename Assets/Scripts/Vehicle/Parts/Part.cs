using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public struct Attachments
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool forward;
    public bool backward;
    public bool this[Vector3Int i]
    {
        get
        {
            if (i == Vector3Int.right)
                return right;
            else if (i == Vector3Int.left)
                return left;
            else if (i == Vector3Int.up)
                return up;
            else if (i == Vector3Int.down)
                return down;
            else if (i == Vector3Int.forward)
                return forward;
            else
                return backward;
        }
        set
        {
            if (i == Vector3Int.right)
                right = value;
            else if (i == Vector3Int.left)
                left = value;
            else if (i == Vector3Int.up)
                up = value;
            else if (i == Vector3Int.down)
                down = value;
            else if (i == Vector3Int.forward)
                forward = value;
            else
                backward = value;
        }
    }
}
public class Part : MonoBehaviour
{
    [SerializeField]
    private PartScriptable partScriptable;
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
    List<int> _proxiesLayersMemory;

    public void TakeDamage(int dmg) => health -= dmg;
    public delegate void ActionDel();
    public virtual List<Tuple<ActionDel, string, KeyCode, int>> GetActions() =>
    new() { new(Activate, "Activate", KeyCode.W, 0), new(DeActivate, "Deactivate", KeyCode.W, 1) };
    //string = action name, KeyCode = action key, int = action type (key up/down)
    public virtual void Activate() => isActive = true;
    public virtual void DeActivate() => isActive = false;

    protected virtual void Awake()
    {
        partData = partScriptable?.data;
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

    public void SetProxiesLayer(int layer) => Proxies.ForEach(_ => _.gameObject.layer = layer);
}
