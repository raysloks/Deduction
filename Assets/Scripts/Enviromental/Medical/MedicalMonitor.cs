using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MedicalMonitor : MinigameInitiator
{
    public Mob[] mobs;

    /*public override bool CanInteract(GameController game)
    {
        return true;
    }*/

    public override void Interact(GameController game)
    {
        minigame.GetComponentInChildren<MedicalMinigame>().UpdateMobs(mobs);
        game.popup.ActivatePopup(minigame, this);
        //foreach (Mob mob in mobs)
            //Debug.Log("Is " + mob.name + " alive? " + (mob.IsAlive == true ? "Yes." : "No."));
    }

    public void SetMobs(Dictionary<ulong, Mob> mobList)
    {
        mobs = mobList.Values.ToArray();
        minigame.GetComponentInChildren<MedicalMinigame>().UpdateMobs(mobs);
    }
}