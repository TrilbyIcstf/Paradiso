using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Camera_Setter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canv = GetComponent<Canvas>();
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        canv.worldCamera = mainCamera;
    }
}
