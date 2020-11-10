using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class EvidenceHandler : MonoBehaviour
{
    public GameObject Content;
    public GameObject PicturePrefab;


    public void AddAllEvidence()
    {
        Debug.Log("Add");
        List<byte[]> b = ScreenshotHandler.GetListOfPicturesTaken();
        foreach(byte[] picture in b)
        {           
            // Texture size does not matter 
            Texture2D sampleTexture = new Texture2D(2, 2);
            // the size of the texture will be replaced by image size
            bool isLoaded = sampleTexture.LoadImage(picture);
            // apply this texure as per requirement on image or material
            GameObject image = Instantiate(PicturePrefab, Content.transform);
            if (isLoaded)
            {
                image.GetComponent<RawImage>().texture = sampleTexture;
            }
        }
        gameObject.SetActive(false);
    }
}
