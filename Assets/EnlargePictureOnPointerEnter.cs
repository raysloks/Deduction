using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnlargePictureOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rt;
    private Vector2 orgWidhtHeight;
    public MainEvidencePicture mp;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        orgWidhtHeight = rt.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(mp.PictureShowing == true)
        {
            rt.sizeDelta = (orgWidhtHeight * 1.5f);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (mp.PictureShowing == true)
        {
            rt.sizeDelta = orgWidhtHeight;
        }
    }
}
