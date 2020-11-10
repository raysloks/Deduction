using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidencePicture : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private MainEvidencePicture med;
    private Texture tex;
    // Start is called before the first frame update
    void Start()
    {
        med = transform.parent.gameObject.GetComponent<MainEvidencePicture>();
        tex = GetComponent<RawImage>().texture;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
        med.SetMainPicture(tex);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Clicked " + name + " GameObject");
        med.LockEvidencePicture();
    }
}
