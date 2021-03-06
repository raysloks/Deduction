﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LineDraw : MonoBehaviour
{

    public GameObject linePrefab;
    private GameObject currentLine;

    public float lineWidth = 0.15f;
    public float minimumVertexDistance = 0.1f; 

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private CircleCollider2D circleCollider;
    public LayerMask mazeLayermask;

    [HideInInspector] public List<Vector2> fingerPositions;


    private bool isLineStarted;
    private Vector2 sizeVector;
    public LayerMask layerMask;
    public TextMeshProUGUI myText;

    private bool isDone;
    private bool canStart = true;

    // Start is called before the first frame update
    void Start()
    {
        isLineStarted = false;
        sizeVector = new Vector2(lineWidth, lineWidth);
        circleCollider = this.transform.GetChild(1).GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame. Checks for player right click and creates /destroy line if conditions are met
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canStart)
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos = new Vector2(tempFingerPos.x, tempFingerPos.y);
            Debug.Log(mousePos.y);
            Collider2D overlap = Physics2D.OverlapPoint(mousePos, mazeLayermask);
            if (overlap == null)
            {
                CreateLine();
                isLineStarted = true;
            }
            else
            {
                string str = overlap.gameObject.name.Substring(0, 4);
                if (str != "Cell")
                {
                    CreateLine();
                    isLineStarted = true;
                }
                Debug.Log("Overlap " + str);
            }
        }

        if (Input.GetMouseButton(0) && isLineStarted)
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(tempFingerPos.x, tempFingerPos.y, 0);
            float distance = Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
            if (distance > minimumVertexDistance)
            {
                UpdateLine(tempFingerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isLineStarted = false;
        }
    }

    void CreateLine()
    {
        //Instantiate currentLine from line prefab and get its renderer/collider
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLine.transform.SetParent(transform);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        //set Line Width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        //clear all mouse/finger positions and add current pos
        fingerPositions.Clear();
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        //Set line pos to current mouse/finger pos
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);

        //Set edgecolliders to pos
        edgeCollider.points = fingerPositions.ToArray();
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        //Update line after moving mouse/finger a set distance
        if(lineRenderer != null)
        {
            fingerPositions.Add(newFingerPos);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
            edgeCollider.points = fingerPositions.ToArray();
            if(currentLine.GetComponent<DrawLineMouseDrag>().GetIsDone() == true)
            {
                isDone = true;
                myText.text = "Done";
            }
        }
        else
        {
            isLineStarted = false;
        }
    }

}
