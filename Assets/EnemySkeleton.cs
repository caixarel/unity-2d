using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{

    bool isAttacking;

    [Header("Player Detectiom")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D PlayerDetection;


    [Header("Move info")]
    [SerializeField] private float moveSpeed;



    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerDetection)
        {

            if (PlayerDetection.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed*0.1f * facingDir, rb.velocity.y);
                isAttacking = false;
            }
            else
            {
                Debug.Log("Attack" + PlayerDetection);
                isAttacking = true;
            }
        }


        if (!isGrounded || isWallDetected) Flip();

        if(!isAttacking) rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        PlayerDetection = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}

