using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    private static ItemButton instance;


    [HideInInspector]public enum Item { None, Camera };
    [HideInInspector]public Item myItem;

    public GameObject itemContainer;
    public GameObject player;
    public GameController gc;

    [Header("Camera")]
    public GameObject buttonItemImage;

    public Sprite CameraSprite;
    public TextMeshProUGUI text;
    private Image myItemImage;

    private int maxPhotos = 3;
    private int photosTaken = 0;

    delegate void Calculation();
    Calculation Click;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
        SetItem(1);
    }
    
    public void ItemClick()
    {
        Click();
    }
    
    private void NoneClick()
    {
        float closestDistance = 2f;
        GameObject childGo = null;
        int index = 0;
        foreach (Transform child in itemContainer.transform)
        {
            if (Vector2.Distance(child.position, player.transform.position) < closestDistance)
            {
                childGo = child.gameObject;
                closestDistance = Vector2.Distance(child.position, player.transform.position);
                Debug.Log("Close " + Vector2.Distance(child.position, player.transform.position));
            }
            index++;

        }
        if (childGo != null)
        {
            
            int item = (int)childGo.GetComponent<ItemContainer>().item;
            if (item != 0)
            {
               SetItem(item);

               childGo.GetComponent<ItemContainer>().ItemTaken();
               PickupCooldown message = new PickupCooldown();
               message.child = index;
               gc.handler.link.Send(message);
            }
            
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
                Click = () => NoneClick();
                myItemImage.enabled = false;
                text.text = "Pickup";
                break;
            case Item.Camera:
                Click = () => CameraClick();
                myItemImage.enabled = true;
                myItemImage.sprite = CameraSprite;
                photosTaken = 0;
                text.text = "";
                break;
        }
    }

    public static void SetItemStatic(int item)
    {
        instance.SetItem(item);
    }
}
