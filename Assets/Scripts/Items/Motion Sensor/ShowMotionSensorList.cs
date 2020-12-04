using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowMotionSensorList : MonoBehaviour
{

    TMP_Dropdown m_Dropdown;


    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
      
    }

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
}
