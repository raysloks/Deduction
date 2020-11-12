using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private static ItemButton instance;


    enum Item { None, Camera };
    Item myItem;

    public GameObject buttonItemImage;
    private Image myItemImage;

    private int maxPhotos = 3;
    private int photosTaken = 0;

    public Sprite CameraSprite;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
        SetItem(1);
    }

    public void Click()
    {
        switch (myItem)
        {
            case Item.None:
                break;
            case Item.Camera:
                CameraClick();               
                break;
        }
    }

    private void CameraClick()
    {
        if (photosTaken < maxPhotos)
        {
            ScreenshotHandler.StartCameraFlash(0.25f);
        }
        photosTaken++;
        if(photosTaken >= maxPhotos)
        {
            SetItem(0);
        }

    }

    public void SetItem(int item)
    {
        myItem = (Item)item;

        switch (myItem)
        {
            case Item.None:
                myItemImage.enabled = false;
                break;
            case Item.Camera:
                myItemImage.enabled = true;
                myItemImage.sprite = CameraSprite;
                photosTaken = 0;
                break;
        }
    }

    public static void SetItemStatic(int item)
    {
        instance.SetItem(item);
    }
}
