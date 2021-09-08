using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float maxJumpForce = 1.0f;
    private float jumpForce;
    public float jumpDropOff = 0.9f;
    //public int jumpPower = 100;
    //public int jumpPowerLeft;
    const float groundedRadius = 0.2f;

    public bool isFacingRight;
    public bool isIdle;
    public bool isOnGround;
    public bool isJumping;
    public bool canJump;
    public GameObject groundCheck;
    public LayerMask whatIsGround;
    public Rigidbody2D playerRb;
    private GameObject player;
    public SpriteRenderer playerSprite;

    public Animator anim;
    public bool isWalking;

    private Vector2 m_Velocity = Vector2.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;


    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check");
        isFacingRight = true;
        //jumpPowerLeft = jumpPower;
        jumpForce = maxJumpForce;
        player = gameObject;
        playerSprite.flipX = isFacingRight;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        bool jumpPressed = Input.GetButton("Jump");

        //new movement code
        Vector2 targetVelocity = new Vector2(moveDirection * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = Vector2.SmoothDamp(playerRb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (moveDirection == 0) 
        { 
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }
        else 
        { 
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
            if (moveDirection > 0 && !isFacingRight) { ChangeFacing(); }
            else if (moveDirection < 0 && isFacingRight) { ChangeFacing(); }
        }


        //jump code. Maybe abstract all this later.
        if (jumpPressed && canJump)
        {
            playerRb.AddForce((Vector2.up * jumpForce), ForceMode2D.Impulse);
            //jumpPowerLeft--;
            jumpForce = jumpForce * jumpDropOff;

            isJumping = true;
            anim.SetBool("isJumping", isJumping);
        }

        if (isJumping && !jumpPressed)
        {
            canJump = false;
        }


        //this is inserted code to detect collision with the ground. Still working on it.
        bool wasGrounded = isOnGround;
        isOnGround = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isOnGround = true;
                canJump = true;
                isJumping = false;
                anim.SetBool("isJumping", isJumping);
                //jumpPowerLeft = jumpPower;
                jumpForce = maxJumpForce;
            }

            if (!wasGrounded)
            {

            }
        }


    }


    void ChangeFacing()
    {
        isFacingRight = !isFacingRight;
        playerSprite.flipX = isFacingRight;
    }
}
