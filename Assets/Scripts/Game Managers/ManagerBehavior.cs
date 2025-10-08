using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBehavior : MonoBehaviour
{
    public GameManager GM
    {
        get { 
            if (_instance != null)
            {
                return _instance;
            } else
            {
                _instance = GameManager.instance;
                return _instance;
            }
        }
        set { _instance = value; }
    }
    private GameManager _instance;
}
