using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Vector2 velocity;

    public void SetUp(int id, string username)
    {
        this.id = id;
        this.username = username;
        velocity = new Vector2();
    }

    protected virtual void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    //private void FixedUpdate()
    //{
    //    SendInputToServer();
    //}

    /// <summary>Sends player input to the server.</summary>
    //private void SendInputToServer()
    //{
    //    ClientSend.SendPlayerStat(transform, velocity);
    //}
}
