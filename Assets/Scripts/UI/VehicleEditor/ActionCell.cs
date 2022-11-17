using System.Collections;
using TMPro;
using UnityEngine;

public class ActionCell : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Part.ActionDel del;
    [SerializeField]
    private TextMeshProUGUI _keyText;
    private KeyCode _key;
    private Part _part;
    public void Init(System.Tuple<Part.ActionDel, string> tuple, Part p)
    {
        Name.text = tuple.Item2;
        del = tuple.Item1;
        _part = p;
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
}
