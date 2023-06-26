using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float power = 30f;
    private Rigidbody2D BombRb2D;
    public Vector2 Direction;
    void Start()
    {
        BombRb2D = GetComponent<Rigidbody2D>();
        Direction += new Vector2(0f, 0.2f);
        BombRb2D.AddForce(Direction * power, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            Destroy(gameObject);
        }
        if(collision.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            Destroy(gameObject);
        }
    }
}
