using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    // Gravity Scale = 2
    private Rigidbody2D PlayerRigidbody;

    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float JumpSpeed = 22;
    [SerializeField] private float DashPower = 3;
    [SerializeField] private float DashingTime = 0.15f;
    [SerializeField] private float DashingCooldown = 0.5f;
    public HealtSystem PlayerHealt;
    public PowerSystem PlayerPower;
    public PointSystem PlayerPoint;

    private Animator PlayerAnimator;
    public Vector2 PlayerDirection;
    private Vector2 MoveInput;
    // Dash
    private bool CanDash;
    public bool IsDashing { get; private set; }
    public bool IsGround { get; set; }
    private bool IsStun;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        IsGround = true;
        CanDash = true;
        IsDashing = false;
    }
    void Update()
    {
        Run();
    }


    private void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        if (!IsDashing && !IsStun)
        {
            Vector2 PlayerVelocity = new Vector2(MoveInput.x * MoveSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = PlayerVelocity;
            Flip();
            //PlayerAnimator.SetFloat("Run", PlayerRigidbody.velocity.magnitude);
        }
    }

    private void OnJump()
    {
        if (IsGround)
        {
            PlayerRigidbody.velocity += new Vector2(0f, JumpSpeed);
            //PlayerAnimator.SetBool("Jump", true);
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
            transform.localScale = new Vector2(1, 1);
            PlayerDirection = new Vector2(1, 0);
        }
        if (MoveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
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
        PlayerRigidbody.AddForce(StunDirection * 13f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.35f);
        IsStun = false;
    }

    public void PlayerOnDamage(Transform EnemyTransform)
    {
        
            PlayerHealt.Damage(15);
            PlayerPower.PowerMinus(5);
            Debug.Log("Healt: " + PlayerHealt.Healt);
            StartCoroutine(Stun(EnemyTransform));
    }
}