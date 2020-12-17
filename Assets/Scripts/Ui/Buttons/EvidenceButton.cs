using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceButton : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject EvidenceScreen;

    public GameObject MainButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void EvidenceClick()
    {
        EvidenceScreen.SetActive(true);
        MainButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
