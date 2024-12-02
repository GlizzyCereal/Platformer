using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("VFX")]
    public GameObject bloodVfx;

    [Header("Movement")]
    public float movementSpeed = 10.0f;
    public float jumpHeight = 3;
    
    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck; //player legs
    public float radius = 0.2f;

    [Header("Jump Mechanics")]
    public float coyoteTime = 0.2f;
    public float jumBufferTime = 0.2f;


    private float jumpBufferCounter;
    private float coyoteCounter;
    private bool isGrounded;
    private Rigidbody2D rb;
    public float inputX;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        
        //OverlapCircle - checks circle area for ground objects
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);

        if(isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        
        if (coyoteCounter > 0 && jumpBufferCounter > 0)
        {
            jumpBufferCounter = 0;

            var jumpForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight * rb.gravityScale);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }   

    void FixedUpdate() //physics update
    {
        rb.velocity = new Vector2(inputX * movementSpeed, rb.velocity.y);
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
        if(other.relativeVelocity.magnitude > 25)
        {
            Instantiate(bloodVfx, transform.position, Quaternion.identity);
        }
    }
}
