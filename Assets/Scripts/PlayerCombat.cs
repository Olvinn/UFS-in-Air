using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public LayerMask enemyLayers;
    public KeyCode keyCode;

    void Update()
    {
        if(Time.time >= nextAttackTime) 
            if(Input.GetKeyDown(keyCode)) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
    }

    void Attack() {
        // animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            if(enemy.gameObject != this.gameObject)
                Debug.Log("Удар по " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
