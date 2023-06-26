using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private PlayerMovement PlayerMove;

    private void Start()
    {
        PlayerMove = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !PlayerMove.IsDashing)
            PlayerMove.PlayerOnDamage(collision.transform);
    }
}
