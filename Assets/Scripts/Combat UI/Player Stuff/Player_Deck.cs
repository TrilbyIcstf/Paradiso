using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Deck : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.CS.SubtractEnergy(12);
    }
}
