using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Manager : ManagerBehavior
{
    private Coroutine activeFade;
    private Action storedAction;

    public void InstantTransmission(string sceneName, bool additive = false)
    {
        SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    public void FadeTransition(string sceneName, bool additive = false, Action fadeAction = null)
    {
        StopCurrentFade();
        this.storedAction = fadeAction;
        this.activeFade = StartCoroutine(PrivFadeTransition(sceneName, additive, fadeAction));
    }

    public void UnloadScene(string sceneName, Action fadeAction = null)
    {
        StopCurrentFade();
        this.storedAction = fadeAction;
        this.activeFade = StartCoroutine(PrivUnloadScene(sceneName, fadeAction));
    }

    private void StopCurrentFade()
    {
        if (this.activeFade != null)
        {
            StopCoroutine(this.activeFade);
            if (this.storedAction != null)
            {
                this.storedAction();
            }
        }
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
            this.storedAction = null;
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
            this.storedAction = null;
        }
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }
}
