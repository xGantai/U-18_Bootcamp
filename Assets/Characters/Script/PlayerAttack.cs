using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Melee Attack Collider")]
    [SerializeField] private Collider2D AttackCollider;
    [SerializeField] private LayerMask EnemyLayer;
    public PowerSystem PlayerPower;
    [HideInInspector] public Animator PlayerAnimator;


    PlayerMovement PlayerMove;
    void Start()
    {
        PlayerPower = new PowerSystem();
        PlayerMove = GetComponent<PlayerMovement>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerPower.SetPower(10);
    }
    private void OnAttack()
    {
        PlayerAnimator.SetTrigger("Attack");
    }
    private void OnHeavyAttack()
    {
        PlayerAnimator.SetTrigger("HeavyAttack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy EnemySC) && AttackCollider.gameObject.activeSelf && collision.CompareTag("Enemy"))
        {
            PlayerMove.EnemyHit = true;
            EnemySC.Damage(PlayerPower.CurrentPower, transform);
        }
    }
    private void AttackAnimFinish()
    {
        PlayerMove.EnemyHit = false;
    }
}