using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalMonitor : MinigameInitiator
{
    public Mob[] mobs;

    public override bool CanInteract(GameController game)
    {
        return true;
    }

    public override void Interact(GameController game)
    {
        foreach (Mob mob in mobs)
            Debug.Log("Is " + mob.name + " alive? " + (mob.IsAlive == true ? "Yes." : "No."));
    }
}