using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFollow : MonoBehaviour
{
    private GameObject container;
    public GameObject image1;
    public GameObject image2;
    // Start is called before the first frame update
    void Start()
    {
        container = transform.parent.gameObject;
        if (container.GetComponent<MinigameHolder>().stayCloseToTarget.GetComponent<StayCloseToTarget>().getIsDone() == true)
        {
            container.GetComponent<MinigameHolder>().stayCloseToTarget.SetActive(false);
            image2.SetActive(true);
            FindObjectOfType<MinigamePopupScript>().MinigameWon();
        }
        else
        {
            image1.SetActive(true);
            container.GetComponent<MinigameHolder>().stayCloseToTarget.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
