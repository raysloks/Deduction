using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMotionSensorList : MonoBehaviour
{

    Dropdown m_Dropdown;


    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown = GetComponent<Dropdown>();
      
    }

    public void addAllOptions(MotionSensor s)
    {
        m_Dropdown.ClearOptions();
        foreach (string Str in s.names)
        {
            Dropdown.OptionData m_NewData = new Dropdown.OptionData();
            m_NewData.text = Str;
            m_Dropdown.options.Add(m_NewData);
        }
    }
}
