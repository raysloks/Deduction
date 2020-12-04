using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MotionSensorEvidence : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    //***** These get filled in during creation process in Evidence handler
    [HideInInspector] public MotionSensor ms;
    //*****


    public Material newMaterial;
    public Sprite Lock;

    private MainEvidencePicture med;
    private Texture tex;
    private Material orginialMaterial;
    private RawImage ri;
    private GameObject lockObject;

    // Start is called before the first frame update
    void Start()
    {
        lockObject = transform.GetChild(0).gameObject;
        med = transform.parent.gameObject.GetComponent<MainEvidencePicture>();
        ri = GetComponent<RawImage>();
     //   orginialMaterial = ri.material;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        med.SetMainSensorList(ms);
        if (med.lockable == true)
        {
          //  ri.material = newMaterial;
            lockObject.SetActive(true);
        }
    }

    //On Pointer Exit Take away the indicator that shows up if lockable
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (med.lockable == true)
        {
           // ri.material = orginialMaterial;
            lockObject.SetActive(false);
        }
    }

    //Tell the main picture that this is the picture you want to send (Look in MainEvidencepicture)
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (med.lockable == true)
        {
            lockObject.SetActive(true);
          //  ri.material = orginialMaterial;
            lockObject.GetComponent<Image>().sprite = Lock;
            med.LockEvidenceSensor(ms);
        }
    }
}
