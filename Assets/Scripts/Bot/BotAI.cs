using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotAI : MonoBehaviour
{
    public Animator animator;
    public Transform[] points;
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

        InvokeRepeating("Patroling", 0, 5);
    }

    void Update()
    {
        // if(!agent.pathPending && agent.remainingDistance < 0.5f)
        //     Patroling();

        // animator.SetFloat("Vertical", points[destPoint].position.x);
        // animator.SetFloat("Horizontal", points[destPoint].position.y);
        // animator.SetFloat("Magnitude", rb.velocity.magnitude);
    }

    IEnumerator DoPatroling() {
        yield return new WaitForSeconds(4f);
    }

    private void Patroling() {
        if(points.Length == 0) return;
        agent.SetDestination(new Vector3(points[destPoint].position.x, points[destPoint].position.y, 0));
        destPoint = (destPoint + 1) % points.Length;
    }

}
