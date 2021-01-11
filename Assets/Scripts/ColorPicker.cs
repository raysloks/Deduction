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

        colors[0] = new Vector3(0.8f, 0f, 0f); //Red
        colors[1] = new Vector3(0.9f, 0.5f, 0f); //Orange
        colors[2] = new Vector3(0.9f, 0.4f, 0.7f); //Pink
        colors[3] = new Vector3(0.25f, 0.6f, 0.1f); //Dark Green
        colors[4] = new Vector3(0f, 0.8f, 0f); //Neon Green
        colors[5] = new Vector3(0f, 0.8f, 0.5f); //Turqoise
        colors[6] = new Vector3(0f, 0.8f, 0.8f); //Light Blue
        colors[7] = new Vector3(0f, 0.4f, 0.6f); //Dark Blue
        colors[8] = new Vector3(0.1f, 0.1f, 0.8f); //Deep Blue
        colors[9] = new Vector3(0.5f, 0f, 1f); //Purple
        colors[10] = new Vector3(0.8f, 0f, 0.8f); //Magenta
        colors[11] = new Vector3(0.6f, 0f, 0.4f); //Dark Magenta
        colors[12] = new Vector3(0.35f, 0.22f, 0f); //Brown
        colors[13] = new Vector3(0.7f, 0.7f, 0.7f); //White
        colors[14] = new Vector3(0.4f, 0.4f, 0.4f); //Grey       
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
