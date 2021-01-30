using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Vector3 velocity;

    public void SetUp(int id, string username)
    {
        this.id = id;
        this.username = username;
        velocity = new Vector3();
    }

    protected virtual void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
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
