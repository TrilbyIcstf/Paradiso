using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Manager : ManagerBehavior
{
    public void InstantTransmission(string sceneName, bool additive = false)
    {
        SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    public void FadeTransition(string sceneName, bool additive = false, Action fadeAction = null)
    {
        StartCoroutine(PrivFadeTransition(sceneName, additive, fadeAction));
    }

    public void UnloadScene(string sceneName, Action fadeAction = null)
    {
        StartCoroutine(PrivUnloadScene(sceneName, fadeAction));
    }

    private IEnumerator PrivFadeTransition(string sceneName, bool additive = false, Action fadeAction = null)
    {
        yield return StartCoroutine(GM.SF.ScreenFade(true));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        yield return new WaitUntil(() => asyncLoad.isDone);
        if (additive)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        yield return new WaitForSeconds(0.25f);
        if (fadeAction != null)
        {
            fadeAction();
        }
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }

    private IEnumerator PrivUnloadScene(string sceneName, Action fadeAction = null)
    {
        yield return StartCoroutine(GM.SF.ScreenFade(true));
        SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.25f);
        if (fadeAction != null)
        {
            fadeAction();
        }
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }
}
