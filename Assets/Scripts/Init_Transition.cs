using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init_Transition : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.instance.EL.RandomizeFloor(5, 5);
        GameManager.instance.TR.InstantTransmission("Scenes/TestMovementRoom");
    }
}
