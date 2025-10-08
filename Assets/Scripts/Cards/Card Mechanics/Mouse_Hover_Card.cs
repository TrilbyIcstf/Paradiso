using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Hover_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject infoBox;

    [SerializeField]
    private Active_Card cardStats;

    private GameObject tempInfoBox;

    private float hoverDuration = 0.75f;
    private Coroutine hoverCoroutine;

    private void OnMouseEnter()
    {
        if (cardStats.GetEffect() == CardEffects.None) { return; }
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        hoverCoroutine = StartCoroutine(DisplayInfoOnHover());
    }

    private void OnMouseExit()
    {
        if (this.hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);
        }
        if (this.tempInfoBox != null)
        {
            Destroy(tempInfoBox);
        }
    }

    IEnumerator DisplayInfoOnHover()
    {
        yield return new WaitForSeconds(this.hoverDuration);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        this.tempInfoBox = Instantiate(infoBox, mousePos, Quaternion.identity);
    }
}
