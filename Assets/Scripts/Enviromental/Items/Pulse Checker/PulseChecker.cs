using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseChecker : MonoBehaviour
{
    // Checks for mobs that are close and activates their heart monitor if they are close

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Mob")
        {
            col.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Mob")
        {
            col.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
