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

    private void Update()
    {
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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Projectile"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                a = GameObject.FindGameObjectWithTag("Player").GetComponent<Player2Health>();
                a.Player2TakeDamage(fireballDamage);

                b = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                b.PlayerTakeDamage(fireballDamage);
            }

            Destroy(this.gameObject);
        }
    }
}
