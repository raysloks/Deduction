using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    private Slider mainSlider;
    private bool isDone = false;
    private bool beenMax = false;
    public GameObject Politican;
    public TextMeshProUGUI text;
    private float WaitForSec = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        mainSlider = GetComponent<Slider>();
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void ValueChangeCheck()
    {
        if (mainSlider.value == mainSlider.maxValue)
        {
            Debug.Log("Max");
            beenMax = true;
            mainSlider.interactable = false;
            Politican.transform.GetChild(0).gameObject.SetActive(false);
            Politican.transform.GetChild(1).gameObject.SetActive(true);
            Politican.transform.GetChild(2).gameObject.SetActive(true);
            StartCoroutine(disableSliderFor(WaitForSec));
        }else if(mainSlider.value == 0 && beenMax == true)
        {
            isDone = true;
            text.text = "Done";
            mainSlider.interactable = false;
        }
    }
    IEnumerator disableSliderFor(float Sec)
    {
        float counter = Sec;

        while (counter > 0)
        {
           // text.text = "Get In Circle " + Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null;
        }

        Politican.transform.GetChild(2).gameObject.SetActive(false);
        mainSlider.interactable = true;

    }
}
