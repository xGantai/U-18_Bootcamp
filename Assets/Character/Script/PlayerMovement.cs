using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Gravity Scale = 2
    private Rigidbody2D PlayerRigidbody;

    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float JumpSpeed = 22;
    [SerializeField] private float DashPower = 3;
    [SerializeField] private float DashingTime = 0.15f;
    [SerializeField] private float DashingCooldown = 0.5f;
    [SerializeField] private BoxCollider2D Footcollider;
    [SerializeField] private HealtSystem PlayerHealt;
    [SerializeField] private PowerSystem PlayerPower;

    private PointSystem PlayerPoint;
    private Animator PlayerAnimator;
    private Vector2 MoveInput;
    // Dash
    private bool CanDash;
    private bool isDashing;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerHealt.SetHealt(PlayerHealt.MaxHealt);
        CanDash = true;
        isDashing = false;
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
        if (!isDashing)
        {
            Vector2 PlayerVelocity = new Vector2(MoveInput.x * MoveSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = PlayerVelocity;
            Flip();
            //PlayerAnimator.SetFloat("Run", PlayerRigidbody.velocity.magnitude);
        }
    }

    private void OnJump()
    {
        if (Footcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            PlayerRigidbody.velocity += new Vector2(0f, JumpSpeed);
            //PlayerAnimator.SetBool("Jump", true);
        }
    }

    private void OnFire()
    {
        //PlayerAnimator.SetTrigger("Attack");
        PlayerPower.PowerBoost(5);
        Debug.Log(PlayerPower.Power);
    }

    private void OnDash()
    {
        if (CanDash)
            StartCoroutine(Dash());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //PlayerAnimator.SetBool("Jump", false);
        }
        if (collision.CompareTag("Enemy") && !isDashing)
        {
            PlayerHealt.Damage(15);
            PlayerPower.PowerMinus(5);
            Debug.Log(PlayerHealt.Healt);
            Debug.Log(PlayerPower.Power);
        }
    }

    private void Flip()
    {
        if (MoveInput.x > 0)
            transform.localScale = new Vector2(1, 1);
        if (MoveInput.x < 0)
            transform.localScale = new Vector2(-1, 1);
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float OriginalGravity = PlayerRigidbody.gravityScale;
        PlayerRigidbody.gravityScale = 0f;
        PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        PlayerRigidbody.gravityScale = OriginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }


}
