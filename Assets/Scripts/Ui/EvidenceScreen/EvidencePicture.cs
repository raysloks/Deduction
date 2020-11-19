using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidencePicture : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private MainEvidencePicture med;
    private Texture tex;
    public Material newMaterial;
    private Material orginialMaterial;
    private RawImage ri;
    [HideInInspector] public List<Vector3> picturePos;
    [HideInInspector] public Vector3 playerPos;
    private RectTransform rTrans;
    public Vector2 change;
    private Vector2 original;
    public Sprite Lock;
    private GameObject lockObject;

    // Start is called before the first frame update
    void Start()
    {
        lockObject = transform.GetChild(0).gameObject;
        med = transform.parent.gameObject.GetComponent<MainEvidencePicture>();
        ri = GetComponent<RawImage>();
        tex = ri.texture;
        orginialMaterial = ri.material;
      //  original = rTrans.anchoredPosition;
        rTrans = (RectTransform)transform.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        med.SetMainPicture(tex);
        if (med.lockable == true)
        {
            ri.material = newMaterial;
            lockObject.SetActive(true);
          //  rTrans.anchoredPosition = change;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(med.lockable == true)
        {
            lockObject.SetActive(true);
            ri.material = orginialMaterial;
            lockObject.GetComponent<Image>().sprite = Lock;
            med.LockEvidencePicture(picturePos, playerPos);
        }       
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
        if (med.lockable == true)
        {
            ri.material = orginialMaterial;
            lockObject.SetActive(false);
          //  rTrans.anchoredPosition = original;
        }
    }
}
