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
    public attachments RotateX()
    {
        var o = this;
        o.up = this.backward;
        o.down = this.forward;
        o.backward = this.up;
        o.forward = this.up;   
        return o;
    }
    public attachments RotateY()
    {
        var o = this;
        o.forward = this.left;
        o.backward = this.right;
        o.left = this.forward;
        o.right = this.backward;
        return o;
    }
    public attachments RotateZ()
    {
        var o = this;
        o.up = this.left;
        o.down = this.right;
        o.right = this.up;
        o.left = this.down;
        return o;
    }
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
    public bool isRoot;
    public bool isActive;
    public void TakeDamage(int dmg)
    {
        health -= dmg;
    }

    public void Activate() => isActive = true;
    public void DeActivate() => isActive = false;
}