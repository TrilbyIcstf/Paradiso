using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class that gives classes that heavily rely on the GameManager a quicker way to reference it
/// </summary>
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
