using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTesting : MonoBehaviour
{
    private void OnMouseDown()
    {
        Enemy_Mouse_Controller.testPause = !Enemy_Mouse_Controller.testPause;
        GameManager.instance.EndCombat();
    }
}
