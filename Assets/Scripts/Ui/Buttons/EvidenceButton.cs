using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidenceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject MainScreen;
    public GameObject EvidenceScreen;
    public GameObject MainButton;

    private RectTransform rt;
    private Vector2 orgWidhtHeight;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        orgWidhtHeight = rt.sizeDelta;
    }
    public void EvidenceClick()
    {
        EvidenceScreen.SetActive(true);
        MainButton.SetActive(true);
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
