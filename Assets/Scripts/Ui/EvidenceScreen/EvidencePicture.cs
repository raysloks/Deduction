using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidencePicture : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [HideInInspector] public int photoIndex;

    public Material newMaterial;
    public Sprite Lock;

    private MainEvidencePicture med;
    private Texture tex;
    private Material orginialMaterial;
    private RawImage ri;
    private GameObject lockObject;

    void Start()
    {
        lockObject = transform.GetChild(0).gameObject;
        med = transform.parent.gameObject.GetComponent<MainEvidencePicture>();
        ri = GetComponent<RawImage>();
        tex = ri.texture;
        orginialMaterial = ri.material;
    }

    //When Pointer Enters picture in scroll view change the main picture to it. If lock button is pressed material will get change too indicate that its lockable on click
    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        med.SetMainPicture(tex);
        if (med.lockable == true)
        {
            ri.material = newMaterial;
            lockObject.SetActive(true);
        }
    }

    //On Pointer Exit Take away the indicator that shows up if lockable
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (med.lockable == true)
        {
            ri.material = orginialMaterial;
            lockObject.SetActive(false);
        }
    }

    //Tell the main picture that this is the picture you want to send (Look in MainEvidencepicture)
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(med.lockable == true)
        {
            lockObject.SetActive(true);
            ri.material = orginialMaterial;
            lockObject.GetComponent<Image>().sprite = Lock;
            med.LockEvidencePicture(photoIndex);
        }       
    }   
}
