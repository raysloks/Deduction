using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowPulseEvidence : MonoBehaviour
{
    //Script to display pulse evidence

    public Image player;
    public Image body;
    public TextMeshProUGUI time;
    public TextMeshProUGUI Description;

    public void DisplayPulseEvidence(PulseCheckerEvidence pce)
    {
        time.text = pce.Time.ToString();
        player.sprite = pce.player.sprite;
        body.sprite = pce.dead.sprite;
        player.color = pce.player.color;
        body.color = pce.dead.color;
        string final = pce.playerName + " Found " + pce.deadName + "\n" + "\n" + "This body is this old (in seconds):";
        Description.text = final;
    }

}
