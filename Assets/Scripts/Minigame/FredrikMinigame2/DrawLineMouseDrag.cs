using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DrawLineMouseDrag : MonoBehaviour
{
    public LineRenderer Line;
    public float lineWidth = 0.04f;
    public float minimumVertexDistance = 0.1f;
    public float winAmount = 4f;
    private float passed = 0f;
    
    private bool isLineStarted;
    private bool isDone = false;

    private List<GameObject> parametersPassed = new List<GameObject>();

    void Start()
    {
        // set the color of the line
         Line.startColor = Color.red;
        Line.endColor = Color.red;

        // set width of the renderer
        Line.startWidth = lineWidth;
        Line.endWidth = lineWidth;

        isLineStarted = false;
        Line.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Line.positionCount = 0;
            Vector2 mousePos = GetWorldCoordinate(Input.mousePosition);
                Line.positionCount = 2;
                Line.SetPosition(0, mousePos);
                Line.SetPosition(1, mousePos);
                isLineStarted = true;
            }

            if (Input.GetMouseButton(0) && isLineStarted)
            {
                Vector3 currentPos = GetWorldCoordinate(Input.mousePosition);
                float distance = Vector2.Distance(currentPos, Line.GetPosition(Line.positionCount - 1));
                if (distance > minimumVertexDistance)
                {
                  //  Debug.Log(distance);
                    UpdateLine();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
            if (parametersPassed.Count > 0)
            {
                foreach (GameObject go in parametersPassed)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }

            passed = 0f;
            isLineStarted = false;
            Destroy(this.gameObject);
            
        }
    }

    private void UpdateLine()
    {
            Line.positionCount++;
            Line.SetPosition(Line.positionCount - 1, GetWorldCoordinate(Input.mousePosition));
    }

    private Vector2 GetWorldCoordinate(Vector3 mousePosition)
    {
            Vector2 mousePos = new Vector2(mousePosition.x, mousePosition.y);
            return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
             if(col.gameObject.tag == "Wall")
             {
            Debug.Log("col");

                Line.positionCount = 0;
            isLineStarted = false;
            if(parametersPassed.Count > 0)
            {
                foreach(GameObject go in parametersPassed)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            passed = 0f;
               Destroy(this.gameObject);

        }
        Debug.Log("col2");


    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            passed++;
            col.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            parametersPassed.Add(col.gameObject);
            if( passed >= winAmount)
            {
                isDone = true;
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
        }


    }
    public bool GetIsDone()
    {
        return isDone;
    }
}

