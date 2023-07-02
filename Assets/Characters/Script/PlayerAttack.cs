using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("Melee Attack Collider")]
    [SerializeField] private Collider2D AttackCollider;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private CharactersSO Character;
    private Animator PlayerAnimator;


    PlayerMovement PlayerMove;
    void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
        PlayerAnimator = GetComponent<Animator>();
    }
    private void OnAttack()
    {
        /*GameObject newbomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
        newbomb.GetComponent<Bomb>().Direction = PlayerMove.PlayerDirection;*/
        PlayerAnimator.SetTrigger("Attack");
    }
    private void OnHeavyAttack()
    {
        PlayerAnimator.SetTrigger("HeavyAttack");
    }
    private void OnCheckPoint()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")&& AttackCollider.gameObject.activeSelf)
        {
            //Debug.Log(collision.name);
            PlayerMove.EnemyHit = true;
        }
    }
    private void AttackAnimFinish()
    {
        PlayerMove.EnemyHit = false;
    }
}