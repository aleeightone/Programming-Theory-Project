using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Range(0, 100)] public int health;
    [Range(0, 100)] public int stamina;
    public float moveSpeed;
    public int score;
    public bool isFacingRight;

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
