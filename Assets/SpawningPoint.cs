using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawningPoint : NetworkBehaviour
{
    public bool isTaken;
    public override void Spawned()
    {
        isTaken = false;
    }
}
