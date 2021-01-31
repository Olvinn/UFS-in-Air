using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Player
{
    protected override void FixedUpdate()
    {
        if (Client.instance.isHost)
            Client.instance.SynchBotPos(id, transform.position, velocity);
    }
}
