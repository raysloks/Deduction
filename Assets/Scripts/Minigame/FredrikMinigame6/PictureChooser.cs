using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureChooser : MonoBehaviour
{
    GameObject mainPicture;
    GameObject removePicture;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.position = GameObject.FindWithTag("Player").transform.position;
        int r = Random.Range(0, (transform.childCount - 1));
        mainPicture = transform.GetChild(r).gameObject;
        Debug.Log(mainPicture.name);
        mainPicture.SetActive(true);
        r = Random.Range(0, (mainPicture.transform.childCount - 1));
        removePicture = mainPicture.transform.GetChild(r).gameObject;
        Debug.Log(removePicture.name);
        removePicture.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
