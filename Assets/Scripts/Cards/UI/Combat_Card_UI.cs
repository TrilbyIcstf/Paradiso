using UnityEngine;

public class Combat_Card_UI : Card_UI_Controller
{
    private void Awake()
    {
        StartCoroutine(OnSpawnInteractionDelay());
    }
}
