using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemContainer : MonoBehaviour
{

    enum Item { None, Camera };
    Item item;
    // Start is called before the first frame update
    void Start()
    {
        item = (Item)UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemTaken()
    {

    }
}
