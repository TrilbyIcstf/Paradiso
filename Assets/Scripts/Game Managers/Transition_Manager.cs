using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions
/// </summary>
public class Transition_Manager : ManagerBehavior
{
    private Coroutine activeFade;
    private Action storedFadeAction;
    private Action storedPostAction;

    public void InstantTransmission(string sceneName, bool additive = false)
    {
        SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    public void FadeTransition(string sceneName, bool additive = false, Action fadeAction = null, Action postAction = null)
    {
        StopCurrentFade();
        this.storedFadeAction = fadeAction;
        this.storedPostAction = postAction;
        this.activeFade = StartCoroutine(PrivFadeTransition(sceneName, additive, fadeAction, postAction));
    }

    public void UnloadScene(string sceneName, Action fadeAction = null, Action postAction = null)
    {
        StopCurrentFade();
        this.storedFadeAction = fadeAction;
        this.storedPostAction = postAction;
        this.activeFade = StartCoroutine(PrivUnloadScene(sceneName, fadeAction, postAction));
    }

    private void StopCurrentFade()
    {
        if (this.activeFade != null)
        {
            StopCoroutine(this.activeFade);
            if (this.storedFadeAction != null)
            {
                this.storedFadeAction();
            }
            if (this.storedPostAction != null)
            {
                this.storedPostAction();
            }
        }
    }

    private IEnumerator PrivFadeTransition(string sceneName, bool additive = false, Action fadeAction = null, Action postAction = null)
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
            this.storedFadeAction = null;
        }
        if (postAction != null)
        {
            postAction();
            this.storedPostAction = null;
        }
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }

    private IEnumerator PrivUnloadScene(string sceneName, Action fadeAction = null, Action postAction = null)
    {
        yield return StartCoroutine(GM.SF.ScreenFade(true));
        SceneManager.UnloadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.25f);
        if (fadeAction != null)
        {
            fadeAction();
            this.storedFadeAction = null;
        }
        if (postAction != null)
        {
            postAction();
            this.storedPostAction = null;
        }
        yield return StartCoroutine(GM.SF.ScreenFade(false));
    }
}
