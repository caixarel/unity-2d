using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;


    [Header("Attack info")]
    private bool isAttacking;
    private int comboCounter;
    private float comboTimeWindow;
    private float comboTime = 1.0f;


    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown ;
    private float dashCooldownTimer;
    private float dashtime;

    private float xInput;

 

    protected override void Start()
    {
        base.Start();   
    }

    protected override void Update()
    {
        base.Update();

        Movement();


        CheckInput();

        dashtime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        FlipController();

        AnimatorControllers();
    }

    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;
        if (comboCounter > 2) comboCounter = 0;

    }

  
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        Jump();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartAttackEvent();

        }


        if (Input.GetKeyDown(KeyCode.Z) && dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashtime = dashDuration;

        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded) return;
        if (comboTimeWindow < 0) comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }
     
    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }

        else if(dashtime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isgrounded", isGrounded);
        anim.SetBool("isDashing", dashtime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

   

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }

    }

   
}
