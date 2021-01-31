using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Combat")]
    private bool isAttack;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRate = 25f;
    float nextAttackTime = 0.25f;
    public LayerMask enemyLayers;
    public KeyCode keyCode;

    [Header("Server")]
    [SerializeField] private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
            animator.SetFloat("lastMoveX", 0);
            animator.SetFloat("lastMoveY", -1);
        }
    }
    void Update()
    {
        ProcessInputs();

        if(isAttack) {
            attackRate -= Time.deltaTime;
            if(attackRate <= 0) {
                animator.SetBool("isAttack", false);
                isAttack = false;
            }
        }

        if(Input.GetKeyDown(keyCode)) {
            attackRate = nextAttackTime;
            Attack();
        }
    }

    private void FixedUpdate() {
        Move();

        if(isAttack) {
            rb.velocity = Vector2.zero;
        } 
    }

    private void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        velocity = new Vector2(moveX, moveY).normalized;
        player.velocity = velocity;
    }

    public void Move() {
        rb.velocity = new Vector2(velocity.x * moveSpeed, velocity.y * moveSpeed);

        if (!animator)
            return;
        animator.SetFloat("Vertical", rb.velocity.x);
        animator.SetFloat("Horizontal", rb.velocity.y);
        animator.SetFloat("Magnitude", rb.velocity.magnitude);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }

    void Attack() {
        animator.SetBool("isAttack", true);
        isAttack = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        List<int> players = new List<int>();
        foreach (var enemy in hitEnemies)
        {
            if (enemy.gameObject != this.gameObject)
            {
                Player player = enemy.GetComponent<Player>();
                if (player)
                    players.Add(player.id);
            }
        }

        Client.instance.HitTargets(players);
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
