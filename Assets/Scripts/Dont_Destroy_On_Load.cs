using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the object not destroyed on load
/// </summary>
public class Dont_Destroy_On_Load : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
