using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Awake()
    {
        health = 1;
        stamina = 1;
        moveSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        Debug.Log("This enemy died!");
    }
}
