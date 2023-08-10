using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private  Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Collision info")]
    [SerializeField] private float groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown ;
    private float dashCooldownTimer;
    private float dashtime;

    private float xInput;

    private bool facingRight = true;
    private bool isGrounded;


    void Start()
    {
        // Debug.Log("hello world");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();

        CheckInput();
        //Debug.Log(xInput);

        CollisionChecks();


        dashtime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        
  

        FlipController();

        AnimatorControllers();
    }

    private void CollisionChecks()
    {
        //check if player is touching ground
        //ground is the layer being passed to the variable what is ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        Jump();


        if (Input.GetKeyDown(KeyCode.Z) && dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashtime = dashDuration;

        }
    }

    private void Movement()
    {
        if(dashtime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
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
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheck));
    }
}
