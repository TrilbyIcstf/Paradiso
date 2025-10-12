using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init_Transition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.EL.RandomizeFloor(5, 5);
        GameManager.instance.TR.InstantTransmission("Scenes/TestMovementRoom");
    }
}
