using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5;
    public Rigidbody2D rb;

    public Animator animator;

    public Player player;

    private Vector2 velocity;
    
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate() {
        Move();
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
    }
}
