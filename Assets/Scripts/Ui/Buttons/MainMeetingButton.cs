using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeetingButton : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject EvidenceScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainClick()
    {
        EvidenceScreen.SetActive(false);

    }
}
