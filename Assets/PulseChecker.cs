using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Mob")
        {
            Debug.Log("MobEnter " + col.gameObject.name);
            col.transform.GetChild(0).gameObject.SetActive(true);
            //     col.transform.Find("HeartMonitor").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Mob")
        {
            Debug.Log("MobExit " + col.gameObject.name);

            col.transform.GetChild(0).gameObject.SetActive(false);
            //   col.transform.Find("HeartMonitor").gameObject.SetActive(false);
        }
    }
}
