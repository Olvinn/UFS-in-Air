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
        Client.instance.SendMessage(packet);
    }
}
