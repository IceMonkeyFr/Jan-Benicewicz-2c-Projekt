using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ustawienia Ruchu")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float jumpHoldForce = 2f;
    public float jumpHoldDuration = 0.2f;

    [Header("Fizyka")]
    public LayerMask groundLayer;
    public Transform[] groundChecks;
    public float groundCheckRadius = 0.2f;

    [Header("Skoki")]
    public int maxJumps = 2; 

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private float moveInput;
    private int jumpCount; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
   
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

 
        isGrounded = false;
        foreach (Transform check in groundChecks)
        {
            if (Physics2D.OverlapCircle(check.position, groundCheckRadius, groundLayer))
            {
                isGrounded = true;
                break;
            }
        }

     
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < maxJumps))
        {
            isJumping = true;
            jumpTimeCounter = jumpHoldDuration;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + jumpHoldForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

       
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundChecks == null) return;

        Gizmos.color = Color.yellow;
        foreach (Transform check in groundChecks)
        {
            if (check != null)
                Gizmos.DrawWireSphere(check.position, groundCheckRadius);
        }
    }
}
