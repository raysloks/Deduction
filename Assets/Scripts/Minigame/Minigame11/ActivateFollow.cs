using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFollow : MonoBehaviour
{
    private GameObject container;
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    // Start is called before the first frame update
    void Start()
    {
        container = transform.parent.gameObject;
        StayCloseToTarget st = container.GetComponent<MinigameHolder>().stayCloseToTarget.GetComponent<StayCloseToTarget>();
        if (st.getIsDone() == true)
        {
            st.SetNumberOfActives(-1);
            if(st.getNumberOfActive() > 0)
            {
                st.resetSlider();
                image3.SetActive(true);
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
            else
            {
                container.GetComponent<MinigameHolder>().stayCloseToTarget.SetActive(false);
                image2.SetActive(true);
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
        }
        else
        {
            st.SetNumberOfActives(1);
            image1.SetActive(true);
            container.GetComponent<MinigameHolder>().stayCloseToTarget.SetActive(true);
        }
    }

}
