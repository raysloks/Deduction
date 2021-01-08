using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowMotionSensorList : MonoBehaviour
{

    List<msbrePrefabImage> imageObjects = new List<msbrePrefabImage>();
    public GameObject content;
    

    // Start is called before the first frame update
    void Start()
    {
        if(content != null)
        {
            imageObjects = content.GetComponentsInChildren<msbrePrefabImage>().ToList();
            Debug.Log("ImageObjects count" + imageObjects.Count);
        }
      
    }
    /*
    public void addAllOptions(MotionSensor s)
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.ClearOptions();
        int index = 0;
        foreach (string Str in s.names)
        {
            TMP_Dropdown.OptionData m_NewData = new TMP_Dropdown.OptionData();
            string final = "Name: " + Str + "  Round Elapsed: " + s.secondsIn[index] + " Sec";
     //       string final = "Name: " + Str + " #Entered: " + (index + 1);
            m_NewData.text = final;
            m_Dropdown.options.Add(m_NewData);
            index++;
        }
        if(s.names.Count == 0)
        {
            TMP_Dropdown.OptionData m_NewData = new TMP_Dropdown.OptionData();
            m_NewData.text = "Noone passed the sensor :(";
            m_Dropdown.options.Add(m_NewData);
        }
      //  m_Dropdown.Show();
     //   m_Dropdown.RefreshShownValue();
    }
    */

    public void addAllOptions(MotionSensor s)
    {
        imageObjects = content.GetComponentsInChildren<msbrePrefabImage>().ToList();
        int index = 0;
        foreach (string Str in s.names)
        {
            if (imageObjects[index].gameObject.activeSelf == false)
            {
                imageObjects[index].gameObject.SetActive(true);                
            }
            imageObjects[index].SetEvidence(Str, (s.totalRoundTime - s.secondsIn[index]), s.playerSprites[index]);
            index++;
        }
        if (s.names.Count == 0)
        {
            imageObjects[0].NoonePassed();
            index++;
        }
        for(int i = index; i < imageObjects.Count; i++)
        {
            if(imageObjects[i].gameObject.activeSelf == true)
            {
                imageObjects[i].gameObject.SetActive(false);
            }
            else
            {
                break;
            }
        }
    }
}
