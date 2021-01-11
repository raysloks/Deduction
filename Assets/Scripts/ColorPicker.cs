using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public GameController game;
    public ColorButton[] Buttons = new ColorButton[15];
    Vector3[] colors = new Vector3[15];
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameController>();

        colors[0] = new Vector3(1f, 0f, 0f);
        colors[1] = new Vector3(1f, 0.5f, 0f);
        colors[2] = new Vector3(1f, 1f, 0f);
        colors[3] = new Vector3(0.4f, 0.6f, 0f);
        colors[4] = new Vector3(0f, 1f, 0f);
        colors[5] = new Vector3(0f, 1f, 0.5f);
        colors[6] = new Vector3(0f, 1f, 1f);
        colors[7] = new Vector3(0f, 0.4f, 0.6f);
        colors[8] = new Vector3(0f, 0f, 1f);
        colors[9] = new Vector3(0.5f, 0f, 1f);
        colors[10] = new Vector3(1f, 0f, 1f);
        colors[11] = new Vector3(0.6f, 0f, 0.4f);
        colors[12] = new Vector3(0.3f, 0.2f, 0f);
        colors[13] = new Vector3(1f, 1f, 1f);
        colors[14] = new Vector3(0.5f, 0.5f, 0.5f);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 PickColor(int i)
    {
        return (colors[i]);
    }

    public void SetButtonColor()
    {
        for (int i = 0; i < 15; i++)
        {
            Buttons[i].RecieveColor(colors[i]);
        }
    }
}
