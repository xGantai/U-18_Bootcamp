using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public float EnemyMoveSpeed;
    public HealtSystem EnemyHealt;
    private Rigidbody2D EnemyRigidbody;
    private bool IsStun;

    void Start()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyHealt.SetHealt(EnemyHealt.MaxHealt);
        IsStun = false;

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }

    public IEnumerator TimedDamage(float Time, int power)
    {
        yield return new WaitForSeconds(Time);
        DamageNotStun(power);
    }

    public void DamageNotStun(int Power)
    {
        EnemyHealt.Damage(Power);
        Debug.Log(EnemyHealt.Healt);
        if (EnemyHealt.Healt <= 0)
            Death();
    }

    public void Damage(int Power, Transform PlayerTransform)
    {
        EnemyHealt.Damage(Power);
        Debug.Log(EnemyHealt.Healt);
        StartCoroutine(EnemyStun(PlayerTransform));
    }

    private IEnumerator EnemyStun(Transform PlayerTransform)
    {
        //EnemyRigidbody.velocity = new Vector2(0f, 0f);
        Vector2 StunDirection = transform.position - PlayerTransform.position;
        StunDirection.y = 0.9f;
        StunDirection.x = (StunDirection.x > 0) ? 0.7f : -0.7f;
        IsStun = true;
        //EnemyAnimator.SetTrigger("TakeHit");
        //PlayerRigidbody.velocity = new Vector2(PlayerDirection.x * AttackForwardMoveValue, 0f);
        //EnemyRigidbody.AddForce(StunDirection * 13f, ForceMode2D.Impulse);
        EnemyRigidbody.velocity = new Vector2(StunDirection.x * 14, 0.1f);
        yield return new WaitForSeconds(0.15f);
        if (EnemyHealt.Healt < 1)
        {
            //EnemyAnimator.SetTrigger("Death");
            Death();
        }
        //EnemyAnimator.SetTrigger("TakeHitFinish");
        IsStun = false;
        EnemyRigidbody.velocity = new Vector2(0f, 0f);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void ImmediateDeath()
    {
        //animation death and Death function
        Death();
    }
}
