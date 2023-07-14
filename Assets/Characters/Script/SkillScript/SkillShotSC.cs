using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotSC : MonoBehaviour
{
    public int Power = 0;
    public float SkillSpeed = 15f;
    private Rigidbody2D SkillRb2D;
    private Enemy Enemys;
    public Vector2 SkillDirection;
    void Start()
    {
        SkillRb2D = GetComponent<Rigidbody2D>();
        SkillRb2D.velocity = new Vector2(SkillSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemys))
        {
            Enemys.DamageNotStun(Power);
        }
    }

    public void StopSkill()
    {
        SkillRb2D.velocity = new Vector2(0f, 0f);
    }

    public void ThisDestroy()
    {
        Destroy(gameObject);
    }
}
