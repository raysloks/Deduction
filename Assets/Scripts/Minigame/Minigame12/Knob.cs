using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;

public class Knob : MonoBehaviour, IDragHandler
{
    float rotationSpeed = 0.2f;
    
    private float targetValue;
    public Slider mySlider;
    private float Counter = 0f;
    private bool checkDone = false;
    private bool isDone = false;
    public AudioSource audio1;
    public AudioSource audio2;
    public TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {
        mySlider.value = 90f;
        targetValue = Random.Range(5, 175);
        audio2.volume = Mathf.Abs(mySlider.value - targetValue) * 0.005f;
        audio1.volume = (175 - Mathf.Abs(mySlider.value - targetValue)) * 0.0002f;
    }
    void Update()
    {
        if (checkDone && !isDone)
        {
            Counter += Time.deltaTime;
            if(Counter > 1f)
            {
                isDone = true;
                Debug.Log("DONE");
                myText.text = "Done";
                audio2.volume = 0f;
                audio1.volume = 0f;
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (isDone)
        {
            return;
        }
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        float rotAmount = YaxisRotation + XaxisRotation * -10f;
        float curRot = transform.localRotation.eulerAngles.z;
        if(transform.rotation.z < 0.7f)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
        }else if(Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount)).z < transform.localRotation.z){
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
        }
        if (transform.localRotation.eulerAngles.z > 0f && transform.localRotation.eulerAngles.z < 90f)
        {
            mySlider.value = Mathf.Abs((transform.localRotation.eulerAngles.z - 90f));
        }
        else
        {
            mySlider.value = Mathf.Abs((transform.localRotation.eulerAngles.z - 360f)) + 90f;
        }
        if(Mathf.Abs(mySlider.value - targetValue) < 4f) 
        {
            checkDone = true;
            audio2.volume = 0f;
            audio1.volume = 0.2f;
        }
        else
        {
            audio2.volume = (Mathf.Abs(mySlider.value - targetValue) / 180 )/ 1.5f;
            audio1.volume = ((180 - Mathf.Abs(mySlider.value - targetValue)) / 180) / 10f;

            Counter = 0f;
            checkDone = false;
        }
    }
}
