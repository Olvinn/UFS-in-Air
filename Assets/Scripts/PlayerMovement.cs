using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5;
    public Rigidbody2D rb;

    public Animator animator;

    private Vector2 direction;
    
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

        direction = new Vector2(moveX, moveY).normalized;
    }

    public void Move() {
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        animator.SetFloat("Vertical", rb.velocity.x);
        animator.SetFloat("Horizontal", rb.velocity.y);
        animator.SetFloat("Magnitude", rb.velocity.magnitude);

    }
}
