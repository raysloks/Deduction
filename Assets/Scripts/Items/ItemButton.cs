using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private static ItemButton instance;


    enum Item { None, Camera };
    Item myItem;

    public GameObject itemContainer;
    public GameObject player;

    [Header("Camera")]
    public GameObject buttonItemImage;

    public Sprite CameraSprite;
    private Image myItemImage;

    private int maxPhotos = 3;
    private int photosTaken = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
        SetItem(0);
    }

    public void Click()
    {
        switch (myItem)
        {
            case Item.None:
                NoneClick();
                break;
            case Item.Camera:
                CameraClick();               
                break;
        }
    }
    private void NoneClick()
    {
        float closestDistance = 2f;
        GameObject childGo; 
        foreach (Transform child in itemContainer.transform)
        {
            if(Vector2.Distance(child.position, player.transform.position) < closestDistance)
            {
                childGo = child.gameObject;
                closestDistance = Vector2.Distance(child.position, player.transform.position);
                Debug.Log("Close " + Vector2.Distance(child.position, player.transform.position));
            }
        }
        if(closestDistance < 2f)
        {
            /*
            int item = (int)childGo.GetComponent<ItemContainer>().item;
            if (item != 0)
            {
               SetItem(item);
               childGo.GetComponent<ItemContainer>().ItemTaken();
            }
            */
        }
    }
    private void CameraClick()
    {
        if (photosTaken < maxPhotos)
        {
            ScreenshotHandler.StartCameraFlash(0.25f, false, Vector3.zero);
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
