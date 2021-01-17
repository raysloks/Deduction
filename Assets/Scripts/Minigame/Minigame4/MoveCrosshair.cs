using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrosshair : MonoBehaviour
{
    Vector3 oldMousePosition;
    public LayerMask lm;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
    }

    //Moves Crosshair;
    void Update()
    {
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
        oldMousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
            ScreenMouseRay();
    }

  
    // Cast a ray from the mouse to the target object
    // Checks for right / wrongtarget
    public void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v, lm);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if(c.tag == "RightTarget")
                {
                    c.gameObject.GetComponent<RightTarget>().Death();
                }else if (c.tag == "WrongTarget")
                {
                    c.gameObject.GetComponent<WrongTarget>().Death();

                }
            }
        }
    }
}
