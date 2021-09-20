using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    
    public float maxJumpForce = 1.0f;
    private float jumpForce;
    public float jumpDropOff = 0.9f;
    const float groundedRadius = 0.2f;

    public float attackTime;
    public float startAttackTime;
    public Transform attackLocation;
    public float attackOffset;
    public static float attackRange = 0.5f;
    public LayerMask enemies;
    public bool isAttacking;


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
        isDead = false;
        SetMoveSpeed(10.0f); //INHERITANCE
        SetHealth(DataController.Instance.GetPlayerHealth()); //INHERITANCE
        Stamina = 5; //ENCAPSULATION
        attackLocation.transform.localPosition = new Vector2(attackOffset, -0.5f);

        player = gameObject;
        playerRb = GetComponent<Rigidbody2D>();
        groundCheck = player.transform.Find("Ground Check").gameObject;
        whatIsGround = LayerMask.GetMask("Ground");
        SetFacingRight(true);
        jumpForce = maxJumpForce;
        
        GameObject spriteObj = player.transform.Find("PlayerSprite").gameObject; //lol this is bad, fix this later...or is it?
        playerSprite = spriteObj.GetComponent<SpriteRenderer>();
        playerSprite.flipX = isFacingRight;
        Color normalColor = new Color(1, 1, 1, 1);
        Color transparentColor = new Color(1, 1, 1, 0.5f); //gonna use this to indicate the player was hit
        //playerSprite.color = transparentColor;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        bool jumpPressed = Input.GetButton("Jump");
        bool attackPressed = Input.GetButton("Fire1");

        Vector2 targetVelocity = new Vector2(moveDirection * moveSpeed, playerRb.velocity.y);
        if (!isDead)
        {
            playerRb.velocity = Vector2.SmoothDamp(playerRb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        
      
        MovementHandler(moveDirection);

        JumpHandler(jumpPressed);

        AttackHandler(attackPressed);

        CheckGrounded();
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("An enemy hit me!");
            DataController.Instance.playerHealth--; //need to go back and sync this later for moving between scenes
            health--;
            GameUIManager.Instance.setHealthUI(health);
        }

        if (collision.gameObject.CompareTag("Pickup"))
        {
            Debug.Log("I touched an item!");
            Destroy(collision.gameObject);
            health++;
            DataController.Instance.playerHealth++;
            DataController.Instance.playerScore++;
            GameUIManager.Instance.setHealthUI(health);
            GameUIManager.Instance.SetScoreUI(DataController.Instance.playerScore);
        }

        if (health == 0)
        {
            Die();
        }
    }

    void MovementHandler(float moveDirection)
    {
        if (moveDirection == 0 && !isDead)
        {
            isWalking = false;
            isIdle = true;
            anim.SetBool("isWalking", isWalking);

        }
        else if (!isDead)
        {
            isWalking = true;
            isIdle = false;
            anim.SetBool("isWalking", isWalking);
            if (moveDirection > 0 && !isFacingRight) { ChangeFacing(); }
            else if (moveDirection < 0 && isFacingRight) { ChangeFacing(); }
        }

    }

    void AttackHandler(bool fire)
    {
        isAttacking = false;
        if (attackTime <= 0)
        {
            if (fire && !isDead)
            {
                Debug.Log("I'm trying to kick!");
                isAttacking = true;
                

                Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);

                for (int i = 0; i < damage.Length; i++)
                {
                    var target = damage[i].gameObject.GetComponent("Enemy");
                    //target.health--;

                    Destroy(damage[i].gameObject);
                }
                attackTime = startAttackTime;
            }
        anim.SetBool("isAttacking", isAttacking);


        }
        else if (attackTime >= 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void CheckGrounded()
    {
        
        bool wasGrounded = isOnGround;
        isOnGround = false;
        //anim.SetBool("isOnGround", isOnGround);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            
            if (colliders[i].gameObject != gameObject)
            {
                
                isOnGround = true;
                canJump = true;
                isJumping = false;
                anim.SetBool("isJumping", isJumping);
                jumpForce = maxJumpForce;
            }

            if (!wasGrounded)
            {
                Debug.Log("This should not happen yet!");
            }
        }
        anim.SetBool("isOnGround", isOnGround);
    }

    void ChangeFacing()
    {
        isFacingRight = !isFacingRight;
        playerSprite.flipX = isFacingRight;
        attackOffset = -attackOffset;
        attackLocation.transform.localPosition = new Vector2(attackOffset, -0.5f);

    }

    void PlayerJump()
    {
        if (!isDead)
        {
            playerRb.AddForce((Vector2.up * jumpForce), ForceMode2D.Impulse);
            jumpForce = jumpForce * jumpDropOff;

            isJumping = true;
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isOnGround", isOnGround);
        }
        
    }

    void JumpHandler(bool jump)
    {
        if (jump && canJump && !isDead)
        {
            PlayerJump();
        }

        if (isJumping && !jump)
        {
            canJump = false;
        }
    }

    public override void Die()
    {
        base.Die();
        anim.SetBool("isDead", isDead);
        Debug.Log("The player has died.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
