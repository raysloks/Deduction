using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowMotionSensorList : MonoBehaviour
{

    List<msbrePrefabImage> imageObjects = new List<msbrePrefabImage>();
    public GameObject content;
    public Image ownerSprite;
    public TextMeshProUGUI rubrik;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        if(content != null)
        {
            imageObjects = content.GetComponentsInChildren<msbrePrefabImage>().ToList();
        }
      
    }

    //Adds the info to the motion sensor list evidence
    public void addAllOptions(MotionSensor s)
    {
        ownerSprite.sprite = s.ownerSprite.sprite;
        ownerSprite.color = s.ownerSprite.color;
        rubrik.text = s.ownerName + "'s Motion Sensor List";

        int index = 0;
        if (s.names.Count == 0)
        {
            imageObjects[0].NoonePassed();
            index++;
        }
        else
        {
            foreach (string Str in s.names)
            {
                if (imageObjects[index].gameObject.activeSelf == false)
                {
                    imageObjects[index].gameObject.SetActive(true);
                }
                imageObjects[index].SetEvidence(Str, (s.totalRoundTime - s.secondsIn[index]), s.playerSprites[index]);
                index++;
                if(index > 28)
                {
                    break;
                }
            }
        }
        
        
        for(int i = index; i < imageObjects.Count; i++)
        {
            if(imageObjects[i].gameObject.activeSelf == true)
            {
                imageObjects[i].gameObject.SetActive(false);
            }
            else
            {
                break;
            }
        }
    }
}
