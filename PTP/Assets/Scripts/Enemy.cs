using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class Enemy : Character
{
    //these are either in-use variables or are fixed needs in planning.
    public int score;
    private GameObject enemy;
    private SpriteRenderer enemySprite;
    private Rigidbody2D enemyRb;
    public GameObject groundCheck;
    public LayerMask whatIsGround;
    
    public GameObject dropCheck;
    public bool willFall;
    public Transform dropCheckLocation;
    public float dropCheckOffset;
    public Vector2 rayDirection;


    private Vector2 m_Velocity = Vector2.zero;
    private float m_MovementSmoothing = .05f;
    private float direction = 1.0f;
    const float groundedRadius = 0.2f;

    //these have not been used yet, delete if needed
    public bool isAlert;


    public void SetScore(int points)
    {
        score = points;
    }

    // Start is called before the first frame update
    void Awake()
    {
        SetHealth(1);
        Stamina = 1;
        SetMoveSpeed(1.0f);
        SetScore(1);
        SetFacingRight(true);

        enemy = gameObject;
        enemyRb = GetComponent<Rigidbody2D>();
        groundCheck = enemy.transform.Find("Ground Check").gameObject;
        dropCheck = enemy.transform.Find("Drop Check").gameObject;
        whatIsGround = LayerMask.GetMask("Ground");
        GameObject spriteObj = enemy.transform.Find("EnemySprite").gameObject; //lol this is bad, fix this later
        enemySprite = spriteObj.GetComponent<SpriteRenderer>();
        dropCheckLocation = dropCheck.transform;
        dropCheckLocation.transform.localPosition = new Vector2(dropCheckOffset, -0.75f);

        Physics2D.queriesStartInColliders = false; //this is also set in project settings so not sure if needed

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ABSTRACTION
        Patrol();
        
        CheckGrounded();

        if (willFall)
        {
            ChangeFacing();
        }

        CheckDropoff();

        //raycast to detect stuff in front of the agent
        if (isFacingRight) { rayDirection = Vector2.right; }
        else if (!isFacingRight) { rayDirection = Vector2.left; }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 0.75f);
        if(hit.collider != null)
        {
            Debug.Log(gameObject.name+": I see a "+hit.collider.name+"! in layer "+hit.collider.gameObject.layer);
            if(hit.collider.gameObject.layer == 6)
            {
                ChangeFacing();
            }

        }


    }

    void Patrol()
    {
        if (isOnGround)
        {
            Vector2 targetVelocity = new Vector2(direction * moveSpeed, enemyRb.velocity.y);
            enemyRb.velocity = Vector2.SmoothDamp(enemyRb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        
    }

    //POLYMORPHISM
    public override void Die()
    {
        Debug.Log("This enemy died!");
    }

    public void ChangeFacing()
    {
        SetFacingRight(!isFacingRight);
        enemySprite.flipX = !isFacingRight;
        direction = -direction;
        dropCheckOffset = -dropCheckOffset;
        dropCheckLocation.transform.localPosition = new Vector2(dropCheckOffset, -0.75f);
    }

    public void CheckGrounded()
    {
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
            }

            if (!wasGrounded)
            {

            }
        }
    }

    public void CheckDropoff()
    {
        willFall = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(dropCheck.transform.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                willFall = false;
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(dropCheckLocation.position, groundedRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rayDirection*0.75f);
    }

}
