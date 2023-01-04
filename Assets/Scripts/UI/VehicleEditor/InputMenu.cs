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
    private PartProxy _partProxy = null;
    private bool _selected;
    private Dictionary<int, int> _originalLayers;
    void Start()
    {
        s_camera = Camera.main;
        s_this = this;
    }
    public void OnMenuOpen()
    {
        _originalLayers = new();
        _parts = GameObject.FindGameObjectsWithTag("Part").ToList().Where(part => part.GetComponent<PartProxy>().Activatable).ToList().ConvertAll(part => { _originalLayers.Add(part.GetInstanceID(), part.layer); part.layer = 6; return part; });

        foreach (Transform c in _actionParent.transform)
            Destroy(c.gameObject);
        _nameText.text = _descText.text = "";
        _selected = false;
    }
    public void OnMenuClose()
    {
        _parts.Where(x => x != null).ToList().ForEach(part => part.layer = _originalLayers[part.GetInstanceID()]);
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
        if (_partProxy != null)
            _partProxy.gameObject.layer = 6;
        _partProxy = part;
        part.gameObject.layer = 9;
        _nameText.text = _partProxy.part.partData.name;
        _descText.text = _partProxy.part.partData.description;

        foreach (Transform c in _actionParent.transform)
            Destroy(c.gameObject);
        _partProxy.part.GetActions().ForEach(a => Instantiate(_actionPrefab,
            Vector3.zero,
            Quaternion.identity,
            _actionParent.transform).GetComponent<ActionCell>().Init(a, _partProxy.part,this));
        if (_partProxy.part.binds.Count == 0) return;
        // PartGroups.RemoveReferences(_part.GetInstanceID());
        // _partProxy.part.binds.Select(x => x.Value).Where(x => x.Item2 == 0).ToList().ForEach(x => PartGroups.DownGroup[x.Item1].RemoveAll(s => s.Item2 == _partProxy.part.GetInstanceID()));
        // _partProxy.part.binds.Select(x => x.Value).Where(x => x.Item2 == 1).ToList().ForEach(x => PartGroups.UpGroup[x.Item1].RemoveAll(s => s.Item2 == _partProxy.part.GetInstanceID()));
        PartGroups.DownGroup.RemoveAllByInstanceId(_partProxy.part.GetInstanceID());
        PartGroups.UpGroup.RemoveAllByInstanceId(_partProxy.part.GetInstanceID());
        _partProxy.part.binds.Clear();

    }
    public void SaveActions()
    {
        if (InputManager.editorMode != EditorMode.input) return;
        foreach (Transform c in _actionParent.transform)
            c.GetComponent<ActionCell>().Save();

    }
}


