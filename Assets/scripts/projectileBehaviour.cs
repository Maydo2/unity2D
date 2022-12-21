using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    private float speed = 9f;
    public bool isFacingRight = true;
    private float fireballDamage = 17f;
    Player2Health a;
    PlayerHealth b;
    public bool hitPlayer = false;

    private void Update()
    {
        a = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player2Health>();
        b = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (isFacingRight)
        {
            transform.position -= -transform.right * Time.deltaTime * speed;
            transform.localScale = new Vector2(1f, 1f);

        }
        else if (!isFacingRight)
        {
            transform.position += -transform.right * Time.deltaTime * speed;
            transform.localScale = new Vector2(-1f, 1f);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Player2") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Projectile"))
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                a.TakeDamage2(fireballDamage);

            }
            if (collision.gameObject.CompareTag("Player"))
            {
                b.TakeDamage(fireballDamage);

            }
            Destroy(this.gameObject);
        }
    }
}
