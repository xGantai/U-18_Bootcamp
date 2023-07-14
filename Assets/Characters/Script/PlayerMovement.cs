using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Gravity Scale = 2
    private Rigidbody2D PlayerRigidbody;

    public float GetMoveSpeed { get { return MoveSpeed; } }
    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float JumpSpeed = 22;
    [SerializeField] private float DashPower = 3;
    [SerializeField] private float DashingTime = 0.15f;
    [SerializeField] private float DashingCooldown = 0.5f;
    public HealtSystem PlayerHealt;
    public PointSystem PlayerPoint;
    [HideInInspector] public float CurrentMoveSpeed;

    private Animator PlayerAnimator;
    [HideInInspector] public Vector2 PlayerDirection;
    [HideInInspector] public float Playerx;
    private Vector2 MoveInput;
    // Dash
    private bool CanDash;
    [HideInInspector] public bool IsDashing;
    [HideInInspector] public bool IsAttacking;
    public bool IsGround { get; set; }
    private bool IsStun;
    [HideInInspector] private float AttackForwardMoveValue = 14f;
    [HideInInspector] public bool EnemyHit;

    void Start()
    {
        Playerx = transform.localScale.x;
        CurrentMoveSpeed = MoveSpeed;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerHealt.SetHealt(50);
        PlayerDirection = new Vector2(1f, 0f);
        IsGround = true;
        CanDash = true;
        IsDashing = false;
    }
    void Update()
    {
        Run();
        AttackForwardMove();
    }


    private void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        if (!IsDashing && !IsStun && !IsAttacking)
        {
            Vector2 PlayerVelocity = new Vector2(MoveInput.x * CurrentMoveSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = PlayerVelocity;
            PlayerAnimator.SetFloat("Run", PlayerRigidbody.velocity.magnitude);
            Flip();
            PlayerAnimator.SetFloat("Fall", PlayerRigidbody.velocity.y);
        }
    }

    private void OnJump()
    {
        if (IsGround)
        {
            PlayerRigidbody.velocity += new Vector2(0f, JumpSpeed);
            PlayerAnimator.SetTrigger("Jump");
        }
    }


    private void OnDash()
    {
        if (CanDash)
            StartCoroutine(Dash());
    }

    private void Flip()
    {
        if (MoveInput.x > 0)
        {
            transform.localScale = new Vector2(Playerx, transform.localScale.y);
            PlayerDirection = new Vector2(1, 0);
        }
        if (MoveInput.x < 0)
        {
            transform.localScale = new Vector2(-Playerx, transform.localScale.y);
            PlayerDirection = new Vector2(-1, 0);
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        float OriginalGravity = PlayerRigidbody.gravityScale;
        PlayerRigidbody.gravityScale = 0f;
        PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        PlayerRigidbody.gravityScale = OriginalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }

    private IEnumerator Stun(Transform EnemyTransform)
    {
        PlayerRigidbody.velocity = new Vector2(0f, 0f);
        Vector2 StunDirection = transform.position - EnemyTransform.position;
        StunDirection.y = 0.9f;
        StunDirection.x = (StunDirection.x > 0) ? 0.7f : -0.7f;
        IsStun = true;
        PlayerAnimator.SetTrigger("TakeHit");
        PlayerRigidbody.AddForce(StunDirection * 13f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        if (PlayerHealt.Healt < 1)
        {
            InputsEnabled(false);
            PlayerAnimator.SetTrigger("Death");
        }
        PlayerAnimator.SetTrigger("TakeHitFinish");
        IsStun = false;
    }

    public void PlayerOnDamage(Transform EnemyTransform)
    {

        PlayerHealt.Damage(15);
        Debug.Log("Healt: " + PlayerHealt.Healt);
        StartCoroutine(Stun(EnemyTransform));
    }

    public void InputsEnabled(bool EnabledData)
    {
        GetComponent<PlayerInput>().enabled = EnabledData;
    }

    public void AttackForwardMove()
    {
        if (IsAttacking && !IsStun && EnemyHit)
            PlayerRigidbody.velocity = new Vector2(PlayerDirection.x * AttackForwardMoveValue, 0f);
    }
}