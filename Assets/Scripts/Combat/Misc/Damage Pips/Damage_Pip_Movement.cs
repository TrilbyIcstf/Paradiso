using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Pip_Movement : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;

    private Transform goal;

    private float speed = 12.5f;
    private float rotSpeed = 30.0f;

    private float rotAccel = 0;

    private float lifespan = 0;

    private bool started = false;

    private void Setup(Transform goal, Transform opponentPos) {
        this.goal = goal;
        float randRotation = Random.Range(-20.0f, 20.0f);

        // Sets direction to oposite of opponent
        this.direction = (transform.position - opponentPos.position).normalized;
        this.direction = this.direction.Rotate(randRotation);

        this.started = true;
    }

    public void SetupAttacker(Transform goal, Transform opponentPos)
    {
        this.rotAccel = 90.0f;
        Setup(goal, opponentPos);
    }

    public void SetupDefender(Transform goal, Transform opponentPos)
    {
        this.rotAccel = 240.0f;
        Setup(goal, opponentPos);
    }

    private void Update()
    {
        if (this.started)
        {
            float time = Time.deltaTime;

            // Check if at goal
            Vector3 velocity = (Vector3)this.direction * time * this.speed;
            float dist = Vector2.Distance(GoalPos(), transform.position);
            if (velocity.magnitude >= dist || GoalDestroyed())
            {
                Destroy(gameObject);
            }

            // Move pip
            Vector3 pos = gameObject.transform.position;
            pos += velocity;
            gameObject.transform.position = pos;

            // Calculate rotation
            float rotDegrees = this.rotSpeed * time;
            Vector2 dirToGoal = GoalPos() - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(this.direction, dirToGoal);
            
            if (Mathf.Abs(angle) <= rotDegrees || velocity.magnitude * 5 >= dist)
            {
                this.direction = dirToGoal.normalized;
            } 
            else
            {
                float rotDirection = Mathf.Sign(angle);
                this.direction = this.direction.Rotate(rotDegrees * rotDirection);
            }

            // Calculate speed changes
            if (Mathf.Abs(angle) > 90)
            {
                this.speed = Mathf.Max(this.speed - (15.0f * time), 3.0f);
            } 
            else
            {
                this.speed += 30.0f * time;
            }
            this.rotSpeed += time * this.rotAccel * Mathf.Max(this.lifespan, 1.0f);



            this.lifespan += time;
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.CUI.GetPipController().RemovePip(gameObject);
    }

    private Vector2 GoalPos()
    {
        if (GoalDestroyed()) { return Vector2.zero; }
        return this.goal.position;
    }

    private bool GoalDestroyed()
    {
        return this.goal == null;
    }
}
