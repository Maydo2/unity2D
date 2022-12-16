using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public projectileBehaviour projectilePrefab;
    public Transform launchOffset;
    public Animator animator;

    public float speed = 6f;
    public float jumpForce = 7.5f;
    public float groundCheckLenght = 1.9f;
    public int airJumps = 1;
    public bool isOnGround;

    private Rigidbody2D rb2D;
    public int remainingJumps;

    private float currentSpeed;
    public bool facesRight;
    public float fireballCooldown;

    public bool hasShot;
    private float animationCastTimer = 0.4f;

    public bool didAttack;
    private float animationAttackTimer = 0.4f;
    private bool canAttack;
    private float downslashforce = 15f;
    private float attackDamage = 12f;

    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        facesRight = false;
        currentSpeed = speed;
        rb2D = GetComponent<Rigidbody2D>();
        remainingJumps = airJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float dir = Input.GetAxis("Horizontal2");

            animator.SetFloat("Speed", 1);

            if (dir > 0)
            {
                facesRight = true;
            }
            else if (dir < 0)
            {
                facesRight = false;
            }

            transform.Translate(transform.right * dir * currentSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }



        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundCheckLenght);
        if (hit.collider != null)
        {
            remainingJumps = airJumps;
            currentSpeed = speed;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsAirAttacking", false);
            isOnGround = true;
            canAttack = true;
        }
        else
        {
            currentSpeed = currentSpeed - 1f * Time.deltaTime;
            animator.SetBool("IsJumping", true);
            isOnGround = false;
        }
        if (!hasShot && !didAttack)
        {
            if (Input.GetKeyDown("[4]"))
            {
                Jump();
            }
        }

        Debug.DrawRay(transform.position, -transform.up * groundCheckLenght, Color.black);

        if (hasShot)
        {
            speed = 0;
            animationCastTimer -= Time.deltaTime;
            if (animationCastTimer <= 0)
            {
                animator.SetBool("IsCasting", false);
                animationCastTimer = 0.4f;
                speed = 6f;
                hasShot = false;
                rb2D.constraints = RigidbodyConstraints2D.None;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        if (didAttack)
        {
            speed = 0;
            animationAttackTimer -= Time.deltaTime;
            if (animationAttackTimer <= 0)
            {
                animator.SetBool("IsAttacking", false);
                animationAttackTimer = 0.4f;
                speed = 6f;
                didAttack = false;
                rb2D.constraints = RigidbodyConstraints2D.None;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        ChangeDirection();
        Shoot();
        Attack();
    }

    private void Jump()
    {
        if (remainingJumps > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            remainingJumps--;
        }
    }

    private void ChangeDirection()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            currentSpeed = 0;
            animator.SetFloat("Speed", 0);
        }
        else
        {
            currentSpeed = speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && facesRight == false)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        if (Input.GetKey(KeyCode.RightArrow) && facesRight == true)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    public void Shoot()
    {
        if (fireballCooldown >= 0)
        {
            fireballCooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown("[8]"))
        {
            if (fireballCooldown <= 0 && !didAttack)
            {
                hasShot = true;
                projectileBehaviour bullet = Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                bullet.isFacingRight = facesRight;
                fireballCooldown = 1f;
                animator.SetBool("IsCasting", true);
                rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown("[7]") && !hasShot)
        {
            if (!isOnGround && canAttack)
            {
                rb2D.AddForce(transform.up * -downslashforce, ForceMode2D.Impulse);
                animator.SetBool("IsAirAttacking", true);
                didAttack = true;
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                canAttack = false;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D player in hitEnemies)
                {
                    player.GetComponent<PlayerHealth>().PlayerTakeDamage(attackDamage);
                }
            }
            else if (canAttack)
            {
                animator.SetBool("IsAttacking", true);
                didAttack = true;
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D player in hitEnemies)
                {
                    player.GetComponent<PlayerHealth>().PlayerTakeDamage(attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
