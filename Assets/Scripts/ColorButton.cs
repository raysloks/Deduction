using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private bool inUse = false;
    private Vector3 color;
    public Button button;
    public ColorPicker colorPicker;


    public int index;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendColor()
    {
        if (!inUse)
        {
            colorPicker.game.ChangeColor(color, index, colorPicker.current);
            inUse = true;
            colorPicker.current = index;
        }
    }

    public void RecieveColor(Vector3 recColor, int i)
    {
        index = i;
        color = recColor;
        Debug.Log(color);
    }

    public void BeingUsed()
    {
        inUse = true;
    }

    public void NoLongerUsed()
    {
        inUse = false;
    }
}
