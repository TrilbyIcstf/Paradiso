using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Energy_Bar_UI : MonoBehaviour
{
    private Image energyBar;

    private Color baseColor;

    private Color pulseColor = Color.red;
    private float pulseDuration = 0.75f;

    private void Awake()
    {
        this.energyBar = GetComponent<Image>();
        this.baseColor = this.energyBar.color;
    }

    public void SetEnergyFill(float fraction)
    {
        this.energyBar.fillAmount = fraction;
    }

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(IEFlashRed());
    }

    private IEnumerator IEFlashRed()
    {
        float timer = 0;
        this.energyBar.color = this.pulseColor;

        while (timer < this.pulseDuration)
        {
            timer += Time.deltaTime;
            this.energyBar.color = Color.Lerp(this.pulseColor, this.baseColor, timer / this.pulseDuration);
            yield return null;
        }

        this.energyBar.color = this.baseColor;
    }
}
