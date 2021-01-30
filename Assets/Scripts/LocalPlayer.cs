using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    protected override void FixedUpdate()
    {
         Client.instance.SynchPlayerPos(transform.position, velocity);
    }
}
