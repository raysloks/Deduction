using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrosshair : MonoBehaviour
{
    Vector3 oldMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
    }

    void Update()
    {
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
        oldMousePosition = Input.mousePosition;
    }
}
