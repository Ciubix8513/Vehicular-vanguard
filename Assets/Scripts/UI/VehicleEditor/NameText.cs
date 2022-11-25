using UnityEngine;
public class NameText : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset = new(100,0,0);
    void Update()=>transform.position = Input.mousePosition + _offset;
}
