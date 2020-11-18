using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;
using System.Linq;

public class EvidenceHandler : MonoBehaviour
{
    public GameObject Content;
    public GameObject PicturePrefab;

    void Start()
    {

        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }
    public void AddAllEvidence()
    {
        Debug.Log("Add");
        List<byte[]> b = ScreenshotHandler.GetListOfPicturesTaken();
        Dictionary<Vector3, List<Vector3>> vec3dict = ScreenshotHandler.GetListOfPicturesPositions();

        int index = 0;
        //cipher.ElementAt(index);
        foreach (byte[] picture in b)
        {        
            
            Texture2D sampleTexture = new Texture2D(2, 2);
            // the size of the texture will be replaced by image size
            bool isLoaded = sampleTexture.LoadImage(picture);
            // apply this texure as per requirement on image or material
            GameObject image = Instantiate(PicturePrefab, Content.transform);
            image.GetComponent<EvidencePicture>().picturePos = vec3dict.ElementAt(index).Value;
            image.GetComponent<EvidencePicture>().playerPos = vec3dict.ElementAt(index).Key;
            index++;
            if (isLoaded)
            {
                image.GetComponent<RawImage>().texture = sampleTexture;
            }
        }
        gameObject.SetActive(false);
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main || pc.phase == GamePhase.Setup)
        {
            this.gameObject.SetActive(true);
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }

        }
    }
}
