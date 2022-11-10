using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSelection : MonoBehaviour
{
    public static Part SelectedPart;
 static   Camera camera;

    private void Awake() 
    {
        camera = Camera.main;
    }

    public static void Select()
    {
        RaycastHit hit;
        if(!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition),out hit,200.0f))return;

        if(hit.collider.CompareTag("Part"))
        {
            SelectedPart = hit.collider.GetComponent<Part>();
        }
    }

}
