using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI for the mana bars in combat
/// </summary>
public class Mana_Bar_UI : MonoBehaviour
{
    private Image manaBar;

    private Color baseColor;

    private Color pulseColor = Color.red;
    private float pulseDuration = 0.75f;

    private void Awake()
    {
        this.manaBar = GetComponent<Image>();
        this.baseColor = this.manaBar.color;
    }

    public void SetManaFill(float fraction)
    {
        this.manaBar.fillAmount = fraction;
    }

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(IEFlashRed());
    }

    private IEnumerator IEFlashRed()
    {
        float timer = 0;
        this.manaBar.color = this.pulseColor;

        while (timer < this.pulseDuration)
        {
            timer += Time.deltaTime;
            this.manaBar.color = Color.Lerp(this.pulseColor, this.baseColor, timer / this.pulseDuration);
            yield return null;
        }

        this.manaBar.color = this.baseColor;
    }
}
