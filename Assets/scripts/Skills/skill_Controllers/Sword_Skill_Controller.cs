using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{

    [SerializeField] private float returnSpeed = 24;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cod;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private float freezeTimeDuration;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBoucing ;
    private int amountOfBounces ;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cod = GetComponent<Collider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _graviyScale,Player _player,float _freezeTimeDuration)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _graviyScale;
        freezeTimeDuration = _freezeTimeDuration;
        if(pierceAmount <=0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupBounce(bool _isBouncing,int _amountOfBounces)
    {
        isBoucing = _isBouncing;
        amountOfBounces = _amountOfBounces;
        enemyTarget = new List<Transform>();
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration,float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();

                        if (enemy)
                        {
                            SwordSkillDamage(enemy);
                        }
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        spinTimer = spinDuration;

        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void BounceLogic()
    {
        if (isBoucing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                Enemy enemy = enemyTarget[targetIndex].GetComponent<Enemy>();

                SwordSkillDamage(enemy);


                targetIndex++;
                amountOfBounces--;

                if (amountOfBounces < 0)
                {
                    isBoucing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count) targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBoucing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning )
        {
            if(!wasStopped)
            {
            StopWhenSpinning();

            }
            return;
        }
        canRotate = false;
        cod.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBoucing && enemyTarget.Count >0) return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
