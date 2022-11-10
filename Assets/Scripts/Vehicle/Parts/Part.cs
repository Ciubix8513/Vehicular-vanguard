using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public int m_health;
    public Vector3 m_size;
    public void TakeDamage(int dmg)
    {
        m_health -= dmg;
    }
    
}
