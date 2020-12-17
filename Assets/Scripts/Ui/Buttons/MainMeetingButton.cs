using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeetingButton : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject EvidenceScreen;
    public GameObject EvidenceButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MainClick()
    {
        EvidenceScreen.SetActive(false);

        EvidenceButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
