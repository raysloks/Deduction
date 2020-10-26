using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbortButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button myButton;
    public Slider mySlider;
    public TextMeshProUGUI myText;
    float counter = 0f;
    private bool pressing = false;
    private bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pressing)
        {
            counter += Time.deltaTime;
           
            if(counter > 0.05f)
            {
                Debug.Log("pressed");
                mySlider.value++;

                counter = 0f;
                if(mySlider.value == mySlider.maxValue)
                {
                    isDone = true;
                    pressing = false;
                    myText.text = "Nuclear Launch Aborted";
                    FindObjectOfType<MinigamePopupScript>().MinigameWon();

                }
            }
        }
    }
    public void OnPointerDown(PointerEventData pe)
    {
        if(isDone == false)
        {
            pressing = true;
        }
    }
    public void OnPointerUp(PointerEventData pe)
    {
        if (isDone == false)
        {
            counter = 0f;
            mySlider.value = 0f;
            pressing = false;
        }
    }
}
