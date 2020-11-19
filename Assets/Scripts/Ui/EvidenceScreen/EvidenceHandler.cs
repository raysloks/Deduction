﻿using System.Collections;
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

    //Adds all the evidence to the scroll view under the evidence tab
    public void AddAllEvidence()
    {
        List<byte[]> b = ScreenshotHandler.GetListOfPicturesTaken();
        List<List<Vector3>> vec3list = ScreenshotHandler.GetListOfPicturesPositions();
        List<Vector3> vec3 = ScreenshotHandler.GetListOfPlayerPositions();

        int index = 0;
        foreach (byte[] picture in b)
        {        
            
            Texture2D sampleTexture = new Texture2D(2, 2);
            bool isLoaded = sampleTexture.LoadImage(picture);

            GameObject image = Instantiate(PicturePrefab, Content.transform);
            image.GetComponent<EvidencePicture>().picturePos = vec3list.ElementAt(index);
            image.GetComponent<EvidencePicture>().playerPos = vec3.ElementAt(index);
            index++;
            if (isLoaded)
            {
                image.GetComponent<RawImage>().texture = sampleTexture;
            }
        }
        gameObject.SetActive(false);
    }

    //End of meeting cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            gameObject.SetActive(true);
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }

        }
    }
}
