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
        colorPicker.game.ChangeColor(color);
    }

    public void RecieveColor(Vector3 recColor)
    {
        color = recColor;
        Debug.Log(color);
    }
}
