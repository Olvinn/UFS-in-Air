using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    protected override void FixedUpdate()
    {
        Client.instance.SynchPlayerStats(transform.position, velocity, isUFS, stunned, killed);
    }
}
