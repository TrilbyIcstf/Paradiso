using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pip_Trail : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitUntilOrTimeout(() =>
        {
            float time = Time.deltaTime;

            Color color = this.sr.color;
            color.a -= 2.0f * time;
            this.sr.color = color;

            Vector3 scale = gameObject.transform.localScale;
            scale.x *= 1 - (0.8f * time);
            scale.y *= 1 - (0.8f * time);
            gameObject.transform.localScale = scale;

            return color.a <= 0;
        }, 0.5f);
        Destroy(gameObject);
    }
}
