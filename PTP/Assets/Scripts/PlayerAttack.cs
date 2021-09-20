using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackTime;
    public float startAttackTime;
    public Transform attackLocation;
    public static float attackRange = 0.5f;
    public LayerMask enemies;
    public bool isAttacking;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isAttacking = false;
        if (attackTime <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
                
                Collider2D[] damage = Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);
                
                for (int i = 0; i < damage.Length; i++)
                {
                    var target = damage[i].gameObject.GetComponent("Enemy");
                    //target.health--;
                    
                    Destroy(damage[i].gameObject);
                }
                attackTime = startAttackTime;
            }

            

        }
        else if (attackTime >= 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
