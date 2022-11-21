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
    private List<GameObject> _parts = new();
    private static Camera s_camera;
    private static InputMenu s_this;
    private Part _part = null;
    private bool _selected;
    void Awake()
    {
        s_camera = Camera.main;
        s_this = this;
    }
    public void OnMenuOpen()
    {
        _parts = GameObject.FindGameObjectsWithTag("Part").ToList().Where(part => part.GetComponent<PartProxy>().Activatable).ToList().ConvertAll(part => { part.layer = 6; return part; });
        foreach (Transform c in _actionParent.transform)
            Destroy(c.gameObject);
        _nameText.text = _descText.text = "";
        _selected = false;
    }
    public void OnMenuClose()
    {
        _parts.ForEach(part => part.layer = 0);
        _selected = false;
    }
    public static void DoRaycast()
    {
        if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition), out var hit, 100.0f, 1 << 6)) return;
        if (!hit.collider.CompareTag("Part")) return;
        s_this.LoadData(hit.collider.GetComponent<PartProxy>());
    }
    private void LoadData(PartProxy part)
    {
        if (_selected)
            SaveActions();
        _selected = true;
        Debug.Log($"Loading data for part {part.part.partData.name}");
        if (_part != null)
            _part.gameObject.layer = 6;
        _part = part.part;
        part.gameObject.layer = 9;
        _nameText.text = _part.partData.name;
        _descText.text = _part.partData.description;

        foreach (Transform c in _actionParent.transform)
            Destroy(c.gameObject);
        _part.GetActions().ForEach(a => Instantiate(_actionPrefab, Vector3.zero, Quaternion.identity, _actionParent.transform).GetComponent<ActionCell>().Init(a, _part));
        if (_part.binds.Count == 0) return;
        _part.binds.Select(x => x.Value).Where(x => x.Item2 == 0).ToList().ForEach(x => PartGroups.DownGroup[x.Item1].RemoveAll(s => s.Item2 == _part.GetInstanceID()));
        _part.binds.Select(x => x.Value).Where(x => x.Item2 == 1).ToList().ForEach(x => PartGroups.UpGroup[x.Item1].RemoveAll(s => s.Item2 == _part.GetInstanceID()));
        _part.binds.Clear();

    }
    public void SaveActions()
    {

        foreach (Transform c in _actionParent.transform)
            c.GetComponent<ActionCell>().Save();
    }
}


