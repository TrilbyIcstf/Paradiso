using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Info_Box : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    private void Update()
    {
        float halfWidth = sr.bounds.size.x / 2;

        Vector3 adjustedMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        adjustedMousePos.z = 0;
        adjustedMousePos.x += halfWidth + 0.5f;

        gameObject.transform.position = adjustedMousePos;
    }
}
