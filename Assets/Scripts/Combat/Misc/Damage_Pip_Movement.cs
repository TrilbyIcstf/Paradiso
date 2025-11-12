using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Pip_Movement : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;

    private Vector2 goal = Vector2.zero;

    private float speed = 12.5f;
    private float rotSpeed = 60.0f;

    private bool started = false;

    public void Setup(Vector2 goal) {
        this.goal = goal;
        float randRotation = Random.Range(-10.0f, 10.0f);

        // Sets direction to oposite of goal
        this.direction = ((Vector2)transform.position - goal).normalized;
        this.direction = this.direction.Rotate(randRotation);

        this.started = true;
    }

    private void Start()
    {
        Setup(new Vector2(20, 8));
    }

    private void Update()
    {
        if (this.started)
        {
            float time = Time.deltaTime;

            // Check if at goal
            Vector3 velocity = (Vector3)this.direction * time * this.speed;
            float dist = Vector2.Distance(this.goal, transform.position);
            if (velocity.magnitude >= dist)
            {
                Destroy(gameObject);
            }

            // Move pip
            Vector3 pos = gameObject.transform.position;
            pos += velocity;
            gameObject.transform.position = pos;

            // Calculate rotation
            float rotDegrees = this.rotSpeed * time;
            Vector2 dirToGoal = this.goal - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(this.direction, dirToGoal);
            if (Mathf.Abs(angle) <= rotDegrees)
            {
                this.direction = dirToGoal.normalized;
            } else
            {
                float rotDirection = Mathf.Sign(angle);
                this.direction = this.direction.Rotate(rotDegrees * rotDirection);
            }

            // Calculate speed changes
            if (Mathf.Abs(angle) > 90)
            {
                this.speed = Mathf.Max(this.speed - (8.0f * time), 6.0f);
            } else
            {
                this.speed += 8.0f * time;
            }
            this.rotSpeed += time * 30.0f;

            Vector3 screenMousePosition = Input.mousePosition;
            this.goal = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePosition.x, screenMousePosition.y, Camera.main.nearClipPlane + 0.1f));
        }
    }
}
