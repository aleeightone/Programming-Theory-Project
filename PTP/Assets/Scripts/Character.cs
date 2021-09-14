using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Range(0, 100)] public int health;
    [Range(0, 100)] public int stamina;
    public float moveSpeed;

    public bool isIdle;
    public bool isOnGround;
    public bool isJumping;
    public bool canJump;
    public bool isFacingRight;

    public void SetHealth(int hp)
    {
        health = hp;
    }

    public void SetStamina(int sta)
    {
        stamina = sta;
    }

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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("I died!");
    }
}
