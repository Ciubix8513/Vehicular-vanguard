using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class InputMenu : MonoBehaviour
{
//----UI fields----
    [SerializeField]
    private GameObject _actionPrefab;
    [SerializeField]
    private GameObject _actionParent;
    [SerializeField]
    TextMeshProUGUI _nameText;
    [SerializeField]
    TextMeshProUGUI _descText;
    
//----UI fields----
    private List<GameObject> _parts;
    private static Camera s_camera;
    private static InputMenu s_this;
    private Part _part = null;
    void Awake()
    {
        s_camera = Camera.main;
        s_this = this;
    }
    public void OnMenuOpen() => _parts = GameObject.FindGameObjectsWithTag("Part").ToList().Where(part => part.GetComponent<Part>().Activatable).ToList().ConvertAll(part => { part.layer = 6; return part; });
    public void OnMenuClose() => _parts.ForEach(part => part.layer = 0);

    public static void DoRaycast()
    {
        if(!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition),out var hit,100.0f,1 << 6))return;
        if(!hit.collider.CompareTag("Part")) return;
        s_this.LoadData(hit.collider.GetComponent<Part>());
    }

    private void LoadData(Part part)
    {
        Debug.Log($"Loading data for part {part.partData.name}");
        if(_part != null)
        _part.gameObject.layer = 6;
        _part = part;   
        _part.gameObject.layer = 9;
        _nameText.text = _part.partData.name;
        _descText.text = _part.partData.description;
    }
}


