using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static_Object_Manager : MonoBehaviour
{
    public static Static_Object_Manager instance
    {
        get { return Static_Object_Manager._instance != null ? Static_Object_Manager._instance : (Static_Object_Manager)FindObjectOfType(typeof(Static_Object_Manager)); }
        set { Static_Object_Manager._instance = value; }
    }
    private static Static_Object_Manager _instance;

    public Card_Base QuillCard;
}
