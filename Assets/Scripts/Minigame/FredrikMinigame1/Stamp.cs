using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Stamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform target;
    private bool noMoreDragging = false;
    private BoxCollider2D collider;
    private bool left = false;
    private bool right = true;
    public LayerMask layerMask;
    private ContactFilter2D cf;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        cf.SetLayerMask(layerMask);

    }


    public void OnBeginDrag(PointerEventData data)
    {
      //  Debug.Log("OnBeginDrag: " + data.position);
        if (noMoreDragging == false)
        {
            data.pointerDrag = this.gameObject;
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

            Ray ray = Camera.main.ScreenPointToRay(data.position);
            //Calculate the distance between the Camera and the GameObject, and go this distance along the ray
            Vector2 rayPoint = ray.GetPoint(Vector2.Distance(transform.position, Camera.main.transform.position));
            //Move the GameObject when you drag it
            transform.position = rayPoint;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
         Collider2D[] results;
        // results.
        //new Vector2(5f, 1.5f)
        results = Physics2D.OverlapBoxAll(transform.position, collider.bounds.size , 0,layerMask);
        /*
        if (results != null)
        {
            Debug.Log(results.gameObject.name);
            results.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            results.gameObject.transform.GetChild(2).position = transform.position;
        }
        */
        
        if(results.Length > 0)
        {
            foreach(Collider2D col in results)
            {
                Debug.Log("HIT: " + col.gameObject.name);
                if(col.gameObject.GetComponent<DragDrop>().GetStamped() == false)
                {
                    col.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                    col.gameObject.transform.GetChild(2).position = (Vector2)transform.position;
                    col.gameObject.GetComponent<DragDrop>().SetStamped(true);
                }              
            }
        }
        
        results = null;
     //   Debug.Log("OnEndDrag: " + data.position );
        
        data.pointerDrag = null;
    }
}
