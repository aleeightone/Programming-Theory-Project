using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Range(0, 100)] public int health;
    [Range(0, 100)] private int stamina;
    public float moveSpeed;

    public bool isIdle;
    public bool isOnGround;
    public bool isJumping;
    public bool canJump;
    public bool isFacingRight;

    //INHERITANCE
    
    public void SetHealth(int hp)
    {
        health = hp;
    }

    public int GetHealth()
    {
        return health;
    }

    //ENCAPSULATION
    public int Stamina { get; set; }
    
    public void SetMoveSpeed(float move)
    {
        moveSpeed = move;
    }

    public void SetFacingRight(bool right)
    {
        isFacingRight = right;
    }

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        if (health == 0)
        {
            Die();
        }
    }
    //POLYMORPHISM
    public virtual void Die()
    {
        Debug.Log("I died!");
    }
}
