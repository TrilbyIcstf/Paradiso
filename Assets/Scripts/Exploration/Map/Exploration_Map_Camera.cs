using System.Collections;
using UnityEngine;

public class Exploration_Map_Camera : MonoBehaviour
{
    public IEnumerator MoveTo(Vector2 pos, float time)
    {
        GameManager.instance.EM.MovementLock = true;

        float t = 0;
        Vector3 initPos = transform.position;
        Vector2 dist = pos - (Vector2)initPos;

        yield return new WaitUntil(() =>
        {
            t += Time.deltaTime;
            float ratio = t / time;
            transform.position = initPos + (Vector3)(dist * ratio);

            return t >= time;
        });

        transform.position = initPos + (Vector3)dist;

        GameManager.instance.EM.MovementLock = false;
    }
}
