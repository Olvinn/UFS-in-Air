using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public Vector3 velocity = new Vector3();
    public bool isUFS = false;
    public bool stunned = false, killed = false;
    public bool isBot = false;

    public void SetUp(int id)
    {
        this.id = id;
        velocity = new Vector3();
    }

    protected virtual void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }
}
