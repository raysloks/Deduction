using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class msbrePrefabImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image playerSprite;
    public TextMeshProUGUI myText;
    public Vector3 scaleChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetEvidence(string s, int time, Sprite sprite)
    {
        string final = s + " Passed " + time + " Seconds ago";
        myText.text = final;
        playerSprite.sprite = sprite;
    }

    public void NoonePassed()
    {
        string final = "Noone Passed The Sensor :(";
        myText.text = final;
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.transform.localScale += scaleChange;
        //Output to console the GameObject's name and the following message
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.localScale -= scaleChange;
        //Output the following message with the GameObject's name
    }

}
