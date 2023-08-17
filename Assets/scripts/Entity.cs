using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Collision Check")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallcheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntitFX FX { get; private set; }

    #endregion

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        FX = GetComponent<EntitFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
         
    }

    public virtual void Damage()
    {
        Debug.Log(gameObject.name + "was damaged");
        FX.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
    }

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Collision
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallcheck.position, new Vector3(wallcheck.position.x + wallCheckDistance * facingDir, wallcheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    protected virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }

    }
    #endregion

    #region Velocity

    public virtual void ZeroVelocity()
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(0, 0);
    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
}

