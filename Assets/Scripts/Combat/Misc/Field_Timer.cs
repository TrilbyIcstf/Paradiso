using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field_Timer : MonoBehaviour
{
    [SerializeField]
    private float timerLength = 1.0f;
    private float timer = -0.2f;

    private Image timerImage;

    private void Awake()
    {
        GameManager.instance.CF.SetFieldTimer(this);
        this.timerImage = GetComponent<Image>();
    }

    public IEnumerator RunTimer()
    {
        this.timerImage.enabled = true;
        this.timerImage.fillAmount = 1;
        yield return new WaitUntil(() =>
        {
            this.timer += Time.deltaTime;
            this.timerImage.fillAmount = 1 - (this.timer / this.timerLength);
            return this.timer >= this.timerLength;
        });
        this.timerImage.enabled = false;
        this.timer = -0.2f;
    }
}
