using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public Animator animator;

    public Player player;

    void FixedUpdate()
    {
        if (!animator)
            return;
        animator.SetFloat("Vertical", player.velocity.x);
        animator.SetFloat("Horizontal", player.velocity.y);
        animator.SetFloat("Magnitude", player.velocity.magnitude);
    }
}
