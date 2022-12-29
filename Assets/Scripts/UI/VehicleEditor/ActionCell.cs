using System.Collections;
using TMPro;
using UnityEngine;

public class ActionCell : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Part.ActionDel del;
    [SerializeField]
    private TextMeshProUGUI _keyText;
    [SerializeField]
    private TMPro.TMP_Dropdown _dropdown;
    private KeyCode _key = KeyCode.W;
    private Part _part;
    [SerializeField]
    int _downOption;
    public void Init(System.Tuple<Part.ActionDel, string,KeyCode,int> tuple, Part p)
    {
        Name.text = tuple.Item2;
        del = tuple.Item1;
        _key = tuple.Item3;
        _dropdown.value = tuple.Item4;
        _part = p;
        _keyText.text = _key.ToString();
        if (p.binds.Count == 0) return;
        _key = p.binds[tuple.Item2].Item1;
        _dropdown.value = p.binds[tuple.Item2].Item2;
        _keyText.text = _key.ToString();
    }

    public void GetKey() => StartCoroutine(getKey());
    private IEnumerator getKey()
    {
        while (!Input.anyKeyDown)
            yield return null;
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKey(key))
            {
                _key = key;
                goto end;
            }
        end:
        _keyText.text = _key.ToString();
    }
    public void Save()
    {
        if (_dropdown.value == _downOption)
        {
            PartGroups.DownGroup.Add(_key,(del,_part.GetInstanceID()));
            if (!_part.binds.ContainsKey(Name.text))
                _part.binds.Add(Name.text, new(_key, 0));
            return;
        }
        PartGroups.UpGroup.Add(_key,(del,_part.GetInstanceID()));
        if (!_part.binds.ContainsKey(Name.text))
            _part.binds.Add(Name.text, new(_key, 1));
    }
}
