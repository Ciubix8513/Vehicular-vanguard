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
    //----Data----
    private List<GameObject> _parts = new();
    private static Camera s_camera;
    public static InputMenu s_this;
    private Part _part = null;
    private bool _selected;
    private static int s_selectedId;
    public static int SelectedId { get => s_selectedId; }
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
        s_selectedId = 0;
    }
    public void OnMenuClose()
    {
        _parts.Where(x => x != null).ToList().ForEach(part => part.layer = _originalLayers[part.GetInstanceID()]);
        _selected = false;
        s_selectedId = 0;
    }
    public static void DoRaycast()
    {
        if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition),
                             out var hit,
                             100.0f,
                             1 << 6)) return;
        if (!hit.collider.CompareTag("Part")) return;
        s_this.LoadData(hit.collider.GetComponent<PartProxy>().part, false);
    }
    public void LoadData(Part part, bool ignoreHistory)
    {
        if (_selected)
            SaveActions();
        _selected = true;
        s_selectedId = part.GetHashCode();
        Debug.Log($"Loading data for part {part.partData.name}");
        if (_part != null)
            _part.SetProxiesLayer(6);
        _part = part;
        part.SetProxiesLayer(9);
        _nameText.text = _part.partData.name;
        _descText.text = _part.partData.description;

        foreach (Transform c in _actionParent.transform)
            Destroy(c.gameObject);
        _part.GetActions().ForEach(a => Instantiate(_actionPrefab,
            Vector3.zero,
            Quaternion.identity,
            _actionParent.transform).GetComponent<ActionCell>().Init(a, _part, this));
        if (_part.binds.Count == 0) goto end;
        PartGroups.DownGroup.RemoveAllByInstanceId(_part.GetInstanceID());
        PartGroups.UpGroup.RemoveAllByInstanceId(_part.GetInstanceID());
        _part.binds.Clear();
    end:
        SaveActions();
        if (!ignoreHistory)
        {
            // print("Making additional save when loading data");
            CarGame.Vehicle.Editor.HistoryManager.ProcessChange("Input menu part loading");
        }
    }
    public void SaveActions()
    {
        if (InputManager.editorMode != EditorMode.input || _part == null) return;
        _part.binds.Clear();
        foreach (Transform c in _actionParent.transform)
            c.GetComponent<ActionCell>().Save();

    }
}


