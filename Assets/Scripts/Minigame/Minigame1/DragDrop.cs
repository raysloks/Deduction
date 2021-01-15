using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using TMPro;
using UnityEngine.EventSystems;
using EventCallbacks;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform target;
    Transform target2;
    private bool noMoreDragging = false;
    private BoxCollider2D collider;
    private bool left = false;
    private bool right = true;
    private Vector2 childpos;
    private Transform child;
    private bool childWith = false;
    private Rigidbody2D rd;
    private bool stamped = false;
    public List<AudioClip> correctSounds;
    public List<AudioClip> wrongSounds;

    void Start()
    {
        if (left == true)
        {
            target = transform.parent.transform.parent.GetChild(1);
            target2 = transform.parent.transform.parent.GetChild(0);
        }
        else if (right == true)
        {
            target = transform.parent.transform.parent.GetChild(0);
            target2 = transform.parent.transform.parent.GetChild(1);
        }
        childpos = Vector2.zero;
        child = transform.GetChild(2);
        collider = GetComponent<BoxCollider2D>();
        rd = GetComponent<Rigidbody2D>();
    }


    public void OnBeginDrag(PointerEventData data)
    {
        //Debug.Log("OnBeginDrag: " + data.position);
        if(noMoreDragging == false)
        {
            data.pointerDrag = gameObject;

            child.position = new Vector3(child.position.x, child.position.y, 0f);
            transform.SetParent(transform.parent.parent.GetChild(4));
            GetComponent<FileMovement>().SetToZero();
            gameObject.layer = 8;
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
            child.position = new Vector3(child.position.x, child.position.y, 0f);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        transform.SetParent(transform.parent.parent.GetChild(2));
        gameObject.layer = 10;
        if (Vector2.Distance(transform.position, target.position) < 200f && stamped == true)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            SoundEvent se = new SoundEvent();
            se.UnitSound = correctSounds;
            se.UnitGameObjectPos = worldPosition;
            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);

            float r = Random.Range(-0.5f, 0.5f);
            float r2 = Random.Range(-0.5f, 0.5f);
            transform.position = (Vector2)target.position + new Vector2(r, r2);
            noMoreDragging = true;
            collider.enabled = false;
            rd.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            transform.parent.gameObject.GetComponent<Spawner>().CheckWinCondition();
            transform.SetParent(transform.parent.GetChild(0));
        }
        else if (Vector2.Distance(transform.position, target2.position) < 200f)
        {

            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SoundEvent se = new SoundEvent();
            se.UnitSound = wrongSounds;
            se.UnitGameObjectPos = worldPosition;
            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
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
        return stamped;
    }
}
