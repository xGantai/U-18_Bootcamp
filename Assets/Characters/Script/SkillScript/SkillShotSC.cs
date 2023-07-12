using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotSC : MonoBehaviour
{
    public int Power = 0;
    public float SkillSpeed = 15f;
    private Rigidbody2D SkillRb2D;
    private Enemy Enemys;
    private Vector2 EndPoint;
    public Vector2 SkillDirection;
    void Start()
    {
        SkillRb2D = GetComponent<Rigidbody2D>();
        SkillRb2D.velocity = new Vector2(SkillSpeed, 0f);
        EndPoint = new Vector2(transform.position.x + (11.5f * SkillDirection.x), transform.position.y);
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, EndPoint) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemys))
        {
            Enemys.DamageNotStun(Power);
        }
    }
}
