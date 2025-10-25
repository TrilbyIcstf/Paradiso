using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the field space that cards are placed on in combat
/// </summary>
public class Card_Holder : MonoBehaviour
{
    [SerializeField]
    internal int position;

    [SerializeField]
    private SpriteRenderer middleArrow;
    [SerializeField]
    private SpriteRenderer leftArrow;
    [SerializeField]
    private SpriteRenderer rightArrow;

    public IEnumerator FlashMiddle(bool playerAdvantage)
    {
        this.middleArrow.flipY = !playerAdvantage;
        yield return StartCoroutine(FlashSprite(this.middleArrow));
    }

    public IEnumerator FlashLeft()
    {
        yield return StartCoroutine(FlashSprite(this.leftArrow));
    }

    public IEnumerator FlashRight()
    {
        yield return StartCoroutine(FlashSprite(this.rightArrow));
    }

    /// <summary>
    /// Flashes a sprite visible for a short period of time.
    /// </summary>
    /// <param name="sprite">The sprite to flash</param>
    private IEnumerator FlashSprite(SpriteRenderer sprite)
    {
        sprite.enabled = true;
        SetAlpha(sprite, 1.5f);
        yield return new WaitUntilOrTimeout(() =>
        {
            float alphaIncrement = Time.deltaTime * 2;
            float curAlpha = sprite.color.a;
            SetAlpha(sprite, curAlpha - alphaIncrement);
            return curAlpha - alphaIncrement <= 0;
        }, 1.0f);
        sprite.enabled = false;
    }

    private void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color rgba = sr.color;
        rgba.a = alpha;
        sr.color = rgba;
    }
}
