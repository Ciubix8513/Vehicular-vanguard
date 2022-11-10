using UnityEngine;
using System;

[Serializable]
public struct PartData
{
    public string name;
    public string description;
    public GameObject prefab;
    public Sprite sprite;
    
}
[CreateAssetMenu(fileName = "Part", menuName = "SOs/Part", order = 0)]
public class PartScriptable : ScriptableObject
{
    public PartData data = new();
}