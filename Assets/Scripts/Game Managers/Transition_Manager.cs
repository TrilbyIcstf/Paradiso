using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Manager : ManagerBehavior
{
    public void InstantTransmission(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void FadeTransition(string sceneName)
    {
        StartCoroutine(PrivFadeTransition(sceneName));
    }

    private IEnumerator PrivFadeTransition(string sceneName)
    {
        yield return StartCoroutine(GM.SF.ScreenFade(true));
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }
}
