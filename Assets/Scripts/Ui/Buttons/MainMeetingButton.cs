using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMeetingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject MainScreen;
    public GameObject EvidenceScreen;
    public GameObject EvidenceButton;
    public GameObject voteButtonContents;
    public GameObject motionSensor;
    public GameObject smokeGrenade;
    public GameObject pulseChecker;
    public RawImage picture;

    private RectTransform rt;
    private Vector2 orgWidhtHeight;
  //  public Vector2 biggerWidthHeight;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        orgWidhtHeight = rt.sizeDelta;
    }

    public void MainClick()
    {
        motionSensor.SetActive(false);
        smokeGrenade.SetActive(false);
        pulseChecker.SetActive(false);
        picture.enabled = false;
        foreach (Transform child in voteButtonContents.transform)
        {
            child.GetComponent<VoteButton>().currentEvidence = false;
            child.localScale = new Vector2(1f,1f);
        }
        EvidenceScreen.SetActive(true);

        EvidenceButton.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        rt.sizeDelta = (orgWidhtHeight * 1.5f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        rt.sizeDelta = orgWidhtHeight;
    }
}
