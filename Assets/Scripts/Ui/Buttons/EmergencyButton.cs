using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmergencyButton : MonoBehaviour
{
    private bool ButtonActive = false;
    private Material m;
    // Start is called before the first frame update
    void Start()
    {
        m = this.GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
     //   myButton.interactable = ButtonActive;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("PlayerEntered");
            m.SetFloat("_OutlineAlpha", 1f);
            col.gameObject.GetComponent<Player>().nearEmergencyButton = true;
            ButtonActive = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("PlayerExited");
            m.SetFloat("_OutlineAlpha", 0f);
            col.gameObject.GetComponent<Player>().nearEmergencyButton = false;
            ButtonActive = false;
        }
    }
}
