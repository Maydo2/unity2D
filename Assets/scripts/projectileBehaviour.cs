using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehaviour : MonoBehaviour
{
    public float speed = 5f;
    public bool isFacingRight = true;

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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
