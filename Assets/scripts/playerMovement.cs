using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce;
    public float groundCheckLength;

    private Rigidbody2D rb2d;
    

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float dir = Input.GetAxis("Horizontal");
            transform.Translate(transform.right * dir * speed * Time.deltaTime);
        }

       

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundCheckLength);

        if (hit.collider != null)
        { 
            if (Input.GetButtonDown("Jump"))
            {
                rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug.Log("hit nothing");
        }

        Debug.DrawRay(transform.position, -transform.up, Color.red);


    }
}
