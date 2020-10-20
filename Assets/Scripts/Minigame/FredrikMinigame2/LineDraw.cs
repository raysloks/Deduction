using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LineDraw : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public GameObject linePrefab;
    private GameObject currentLine;

    public float lineWidth = 0.15f;
    public float minimumVertexDistance = 0.1f; 

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private CircleCollider2D circleCollider;

    [HideInInspector] public List<Vector2> fingerPositions;


    private bool isLineStarted;
    private Vector2 sizeVector;
    public LayerMask layerMask;
    private TextMeshProUGUI myText;

    private bool isDone;
    private bool canStart = true;

    // Start is called before the first frame update
    void Start()
    {
        isLineStarted = false;
        sizeVector = new Vector2(lineWidth, lineWidth);
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        circleCollider = this.transform.GetChild(1).GetComponent<CircleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canStart)
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(tempFingerPos.x, tempFingerPos.y, 0);
            Debug.Log(mousePos.y);
            if(Vector2.Distance(transform.position, mousePos) < 1.4f)
            {
                CreateLine();
                isLineStarted = true;
            }
            
            
        }
        if (Input.GetMouseButton(0) && isLineStarted)
        {
            Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(tempFingerPos.x, tempFingerPos.y, 0);
            float distance = Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]);
            if (distance > minimumVertexDistance)
            {
                UnityEngine.Debug.Log("UP");
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
                this.isDone = true;
                myText.text = "Done";
            }
        }
        else
        {
            isLineStarted = false;
        }

    }

    public void OnPointerEnter(PointerEventData ped)
    {
        canStart = true;
        Debug.Log("Pointer Enters");
    }
    public void OnPointerExit(PointerEventData ped)
    {
        canStart = false;

        Debug.Log("Pointer exits");

    }
}
