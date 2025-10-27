using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement during exploration
/// </summary>
public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float maxMoveSpeed = 10.0f;
    private float moveSpeed = 0.0f;
    private float moveTime = 0.0f;

    private Animator animator;

    private Rigidbody2D rb;

    private Vector2 inputDirection;
    private Vector2 velocityDirection;

    private bool moveLocked = false;

    private void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.moveLocked = GameManager.instance.EP.GetMovementLock();
    }

    void Update()
    {
        this.inputDirection.x = Input.GetAxisRaw("Horizontal");
        this.inputDirection.y = Input.GetAxisRaw("Vertical");
        this.inputDirection.Normalize();
    }

    private void FixedUpdate()
    {
        if (this.moveLocked) {
            this.rb.velocity = Vector2.zero;
            this.animator.SetBool("Walking", false);
            return; 
        }

        if (this.inputDirection.magnitude > 0)
        {
            this.moveTime += Time.deltaTime * 3.5f;
            this.velocityDirection = this.inputDirection;
            this.animator.SetBool("Walking", true);
        } else
        {
            this.moveTime -= Time.deltaTime * 5.5f;
            this.animator.SetBool("Walking", false);
        }
        this.moveTime = Mathf.Clamp(this.moveTime, 0, 1);
        this.moveSpeed = Mathf.Lerp(0.0f, this.maxMoveSpeed, this.moveTime);
        this.rb.velocity = this.velocityDirection * this.moveSpeed;
    }

    public void SetMoveLock(bool val)
    {
        this.moveLocked = val;
    }
}
