using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EmergencyButton : MonoBehaviour
{
    private bool ButtonActive = false;
    private Material m;
    private TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        m = this.GetComponent<SpriteRenderer>().material;
        text = this.transform.GetChild(0).transform.gameObject.GetComponent<TextMeshPro>();
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
            text.color = Color.green;
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
            text.color = Color.black;

            col.gameObject.GetComponent<Player>().nearEmergencyButton = false;
            ButtonActive = false;
        }
    }
}
