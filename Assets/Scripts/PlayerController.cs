using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        Packet packet = new Packet(Command.synchPosPlayer);
        packet.Write(Client.instance.id);
        packet.Write(transform.position);
        Client.instance.SendMessage(packet);
    }
}
