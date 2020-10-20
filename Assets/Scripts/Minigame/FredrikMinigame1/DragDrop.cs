using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using TMPro;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform target;
    private bool noMoreDragging = false;
    private BoxCollider2D collider;
    private bool left = false;
    private bool right = true;
    private Vector2 childpos;
    private Transform child;
    private bool childWith = false;
    private Rigidbody2D rd;
    private bool stamped = false;

    void Start()
    {
        if (left == true)
        {
            target = transform.parent.transform.parent.GetChild(1);
        }
        else if (right == true)
        {
            target = transform.parent.transform.parent.GetChild(0);

        }
        childpos = Vector2.zero;
        child = transform.GetChild(2);
        collider = GetComponent<BoxCollider2D>();
        rd = this.GetComponent<Rigidbody2D>();

    }


    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag: " + data.position);
        if(noMoreDragging == false)
        {
            data.pointerDrag = this.gameObject;

            child.position = new Vector3(child.position.x, child.position.y, 0f);
        }
        else
        {
           data.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
          //  Debug.Log("Dragging:" + data.position);

            Ray ray = Camera.main.ScreenPointToRay((Vector2)data.position);
            //Calculate the distance between the Camera and the GameObject, and go this distance along the ray
            Vector2 rayPoint = (Vector2)ray.GetPoint(Vector2.Distance(transform.position, Camera.main.transform.position));
            //Move the GameObject when you drag it
            transform.position = rayPoint;
            child.position = new Vector3(child.position.x, child.position.y, 0f);

        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if(Vector2.Distance(transform.position, target.position) < 1f && stamped == true)
        {
            Debug.Log("hit it");
            float r = Random.Range(-0.5f, 0.5f);
            float r2 = Random.Range(-0.5f, 0.5f);
            transform.position = (Vector2)target.position + new Vector2(r, r2);
            noMoreDragging = true;
            collider.enabled = false;
            rd.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            transform.parent.gameObject.GetComponent<Spawner>().CheckWinCondition();
            transform.SetParent(transform.parent.GetChild(0));

        }
        else
        {

            Debug.Log("OnEndDrag: " + data.position +" target: " + target.position + " distance " + Vector2.Distance(transform.position, target.position));
        }
        data.pointerDrag = null;
    }
    public void RightOrLeft(string s)
    {
        if(s == "Right")
        {
            right = true;
        }
        else
        {
            left = true;
        }
    }
    public void SetStamped(bool s)
    {
        stamped = true;
    }
    public bool GetStamped()
    {
        return this.stamped;
    }
}
