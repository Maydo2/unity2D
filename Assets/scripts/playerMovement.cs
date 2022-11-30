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

    private Rigidbody2D rb2D;
    public int remainingJumps;

    private float currentSpeed;
    public bool facesRight;
    public float fireballCooldown;

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

            animator.SetFloat("speed", 1);

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
            animator.SetFloat("speed", 0);
        }
        


        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundCheckLenght);
        if (hit.collider != null)
        {
            remainingJumps = airJumps;
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = currentSpeed - 1f * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        Debug.DrawRay(transform.position, -transform.up * groundCheckLenght, Color.black);

        ChangeDirection();
        Shoot();
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
            if (fireballCooldown <= 0)
            {
                projectileBehaviour bullet = Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                bullet.isFacingRight = facesRight;
                fireballCooldown = 1f;
            }
        }
    }
}
