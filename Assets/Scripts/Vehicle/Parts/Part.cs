using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct attachments
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
    public PartData partData;
    public int health;
    public Vector3 m_size;
    public attachments attachedParts;
    public Vector3Int parentFace;
    public Part parentPart;
    public bool isRoot;
    public bool Activatable;
    public bool isActive;
    public void TakeDamage(int dmg)=>health -= dmg;
    public delegate void ActionDel();
    public virtual List<Tuple<ActionDel, string,KeyCode,int>> GetActions()=> 
    new(){new (Activate,"Activate",KeyCode.W,0),new (DeActivate,"Deactivate",KeyCode.W,1)};
    public Dictionary<string,Tuple<KeyCode,int>> binds = new();
    public virtual void Activate() => isActive = true;
    public virtual void DeActivate() => isActive = false;
}
