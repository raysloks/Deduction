using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowSmokeGrenade : MonoBehaviour
{
    public Image player;
    public TextMeshProUGUI description;
    public TextMeshProUGUI area;

    public void DisplaySmokeEvidence(SGEvidence sge)
    {
        player.sprite = sge.player.sprite;
        player.color = sge.player.color;
        description.text = sge.playerName + " Fired A Smoke Grenade In";
        area.text = "Area: " + sge.area;
    }

}
