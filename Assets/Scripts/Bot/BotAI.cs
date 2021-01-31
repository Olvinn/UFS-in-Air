using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotAI : MonoBehaviour
{
    public Animator animator;
    public Transform[] points;
    public Player player;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        if(points.Length != 0)
            this.transform.position = points[0].position;

        InvokeRepeating("Patroling2", 0, 5);
    }

    void Update()
    {
        if (Client.instance.isHost)
            player.velocity = agent.velocity;
        else
            agent.velocity = player.velocity;

        // if(!agent.pathPending && agent.remainingDistance < 0.5f)
        //     Patroling();

        animator.SetFloat("Vertical", player.velocity.x);
        animator.SetFloat("Horizontal", player.velocity.y);
        animator.SetFloat("Magnitude", player.velocity.magnitude);
    }

    IEnumerator DoPatroling() {
        yield return new WaitForSeconds(4f);
    }

    private void Patroling() {
        if(points.Length == 0) return;
        agent.SetDestination(new Vector3(points[destPoint].position.x, points[destPoint].position.y, 0));
        destPoint = (destPoint + 1) % points.Length;
    }
    private void Patroling2()
    {
        if (agent.velocity.magnitude > 0) return;
        float angle = Random.value * 360;
        Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        float r = 10;
        agent.SetDestination(transform.position + dir * r);
    }

}
