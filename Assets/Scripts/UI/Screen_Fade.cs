using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen_Fade : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        this.sr = GetComponent<SpriteRenderer>();
        ScaleToScreen();
        GameManager.instance.SF = this;
        StartCoroutine(ScreenFade(false));
    }
    
    private void ScaleToScreen()
    {
        Camera mainCamera = Camera.main;

        float screenHeight = mainCamera.orthographicSize * 2.0f;
        float screenWidth = screenHeight / Screen.height * Screen.width;

        float spriteWidth = this.sr.sprite.bounds.size.x;
        float spriteHeight = this.sr.sprite.bounds.size.y;

        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }

    public IEnumerator ScreenFade(bool fadeIn)
    {
        int fadeDirection = fadeIn ? 2 : -2;
        Color srColor = this.sr.color;
        srColor.a = fadeIn ? 0.0f : 1.0f;
        yield return new WaitUntil(() =>
        {
            srColor.a += Time.deltaTime * fadeDirection;
            this.sr.color = srColor;
            if (fadeIn)
            {
                return srColor.a >= 1.0f;
            } else
            {
                return srColor.a <= 0.0f;
            }
        });
    }
}
