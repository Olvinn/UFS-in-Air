using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    private void FixedUpdate()
    {
        Packet packet = new Packet(Command.synchPosPlayer);
        packet.Write(Client.instance.id);
        packet.Write(transform.position);
        packet.Write(velocity);
        Client.instance.SendMessage(packet);
    }
}
