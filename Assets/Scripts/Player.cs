using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("VFX")]
    public GameObject bloodVfx;

    [Header("Movement")]
    public float movementSpeed = 10.0f;
    public float jumpHeight = 3f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float radius = 0.2f;

    [Header("Jump Mechanics")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public int maxJumps = 2;
    private int jumpsLeft;
    private float jumpBufferCounter;
    private float coyoteCounter;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float inputX;

    [Header("Dash Mechanics")]
    private bool isDashing;
    private float dashTime;
    private float dashCooldownTime;

    private Health health;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip walkSound;
    private AudioSource audioSource;

    private bool isWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            jumpsLeft = maxJumps;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((coyoteCounter > 0 || jumpsLeft > 0) && jumpBufferCounter > 0)
        {
            jumpBufferCounter = 0;
            float jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight * rb.gravityScale);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            if (!isGrounded)
            {
                jumpsLeft--;
            }

            audioSource.PlayOneShot(jumpSound);
        }

        // Dash mechanics
        if (Input.GetButtonDown("Fire3") && dashCooldownTime <= 0f)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTime = dashCooldown;
        }

        if (isDashing)
        {
            rb.velocity = new Vector2(dashSpeed * inputX, rb.velocity.y);
            dashTime -= Time.deltaTime;

            if (dashTime <= 0f)
            {
                isDashing = false;
            }
        }
        else if (!isDashing)
        {
            rb.velocity = new Vector2(inputX * movementSpeed, rb.velocity.y);
        }

        dashCooldownTime -= Time.deltaTime;

        // Walking sound
        if (isGrounded && Mathf.Abs(inputX) > 0 && !isWalking)
        {
            isWalking = true;
            audioSource.clip = walkSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if ((Mathf.Abs(inputX) == 0 || !isGrounded) && isWalking)
        {
            isWalking = false;
            audioSource.Stop();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, radius);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Fall damage
        if (other.relativeVelocity.magnitude > 25f)
        {
            Instantiate(bloodVfx, transform.position, Quaternion.identity);
            health.TakeDamage(0.5f);
        }
    }
}