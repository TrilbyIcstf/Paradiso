using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Pip_Movement : MonoBehaviour
{
    private Vector3 direction = Vector2.zero;

    private Vector2 goal = Vector2.zero;

    private float speed = 12.5f;

    private float pipCounter = 0;
    private float pipRate = 0.05f;

    private bool started = false;

    [SerializeField]
    private GameObject trailPip;

    public void Setup(Vector2 direction, Vector2 goal) {
        this.direction = direction.normalized;
        this.goal = goal;

        this.started = true;
    }

    private void Start()
    {
        Setup(new Vector2(1, 1), new Vector2(40, 40));
    }

    private void Update()
    {
        if (this.started)
        {
            float time = Time.deltaTime;

            this.pipCounter += time;
            if (this.pipCounter >= this.pipRate)
            {
                GameObject pip = Instantiate(this.trailPip, gameObject.transform.position, Quaternion.identity);
                this.pipCounter = 0;
            }

            Vector3 pos = gameObject.transform.position;
            pos += this.direction * time * this.speed;
            gameObject.transform.position = pos;
        }
    }
}
