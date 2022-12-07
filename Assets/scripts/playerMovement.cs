using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public projectileBehaviour projectilePrefab;
    public Transform launchOffset;
    public Animator animator;

    public float speed = 10f;
    public float jumpForce = 7f;
    public float groundCheckLenght = 1.95f;
    public int airJumps = 2;
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

    // Start is called before the first frame update
    void Start()
    {
        facesRight = true;
        currentSpeed = speed;
        rb2D = GetComponent<Rigidbody2D>();
        remainingJumps = airJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float dir = Input.GetAxis("Horizontal");

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
            isOnGround = true;
        }
        else
        {
            currentSpeed = currentSpeed - 1f * Time.deltaTime;
            animator.SetBool("IsJumping", true);
            isOnGround = false;
        }
        if (!hasShot && !didAttack)
        {
            if (Input.GetButtonDown("Jump"))
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
                speed = 10;
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
                speed = 10;
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
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            currentSpeed = 0;
            animator.SetFloat("Speed", 0);
        }
        else
        {
            currentSpeed = speed;
        }
        if (Input.GetKey(KeyCode.A) && facesRight == false)
        {
            transform.localScale = new Vector2(-1.5f, 1.5f);
        }
        if (Input.GetKey(KeyCode.D) && facesRight == true)
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
        }
    }

    public void Shoot()
    {
        if (fireballCooldown >= 0)
        {
            fireballCooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (fireballCooldown <= 0 && !didAttack)
            {
                hasShot = true;
                projectileBehaviour bullet = Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                bullet.isFacingRight = facesRight;
                fireballCooldown = 1f;
                animator.SetBool("IsCasting", true);
                rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.G) && !hasShot)
        {
            if (!isOnGround)
            {
                /*rb2D.AddForce();*/
                animator.SetBool("IsAttacking", true);
                didAttack = true;
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                animator.SetBool("IsAttacking", true);
                didAttack = true;
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            
        }
    }
}
