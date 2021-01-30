using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    protected override void FixedUpdate()
    {
        if(!Client.instance)
            Client.instance.SynchPlayerStats(transform.position, velocity, isUFS, stunned, killed);
    }
}
