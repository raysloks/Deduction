using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using EventCallbacks;

public class Stamp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform target;
    private bool noMoreDragging = false;
    private BoxCollider2D collider;
    private bool left = false;
    private bool right = true;
    public LayerMask layerMask;
    private ContactFilter2D cf;
    public List<AudioClip> stampSounds;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        cf.SetLayerMask(layerMask);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        //Debug.Log("OnBeginDrag: " + data.position);
        if (noMoreDragging == false)
        {
            data.pointerDrag = gameObject;
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
            transform.position = data.position;
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
        
        if (results.Length > 0)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SoundEvent se = new SoundEvent();
            se.UnitSound = stampSounds;
            se.UnitGameObjectPos = worldPosition;
            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
            foreach (Collider2D col in results)
            {
                Debug.Log("HIT: " + col.name);
                if (col.GetComponent<DragDrop>().GetStamped() == false)
                {
                    col.transform.GetChild(2).gameObject.SetActive(true);
                    col.transform.GetChild(2).position = (Vector2)transform.position;
                    col.GetComponent<DragDrop>().SetStamped(true);
                }              
            }
        }
        
        //Debug.Log("OnEndDrag: " + data.position );
        
        data.pointerDrag = null;
    }
}
