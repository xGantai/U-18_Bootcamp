using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("Melee Attack Collider")]
    [SerializeField] private Collider2D AttackCollider;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private CharactersSO Character;
    [SerializeField] private GameObject BombPrefab;


    PlayerMovement PlayerMove;
    void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
    }
    private void OnFire()
    {
        
        GameObject newbomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
        newbomb.GetComponent<Bomb>().Direction = PlayerMove.PlayerDirection;

        // Melee Attack
        //PlayerAnimator.SetTrigger("Attack");
        /*Collider2D[] HitColliders = new Collider2D[10];
        ContactFilter2D ContactFilter = new ContactFilter2D();
        ContactFilter.useTriggers = true;
        ContactFilter.SetLayerMask(EnemyLayer);
        int HitCount = Physics2D.OverlapCollider(AttackCollider, ContactFilter, HitColliders);
        if (HitCount > 0)
        {
            for (int i = 0; i < HitCount; i++)
            {
                Debug.Log(HitColliders[i].name);
            }
        }*/

    }
    private void OnCheckPoint()
    {
        
    }
}