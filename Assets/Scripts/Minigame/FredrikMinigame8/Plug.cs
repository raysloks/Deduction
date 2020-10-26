using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Plug : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform child1;
    private Transform child2;

    public Sprite child1sprite;
    public Sprite child2sprite;
    public TextMeshProUGUI text;
    private bool isDone = false;
    private bool noMoreDragging = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform canvas = transform.parent;
        child1 = canvas.GetChild(1).transform;
        child2 = canvas.GetChild(2).transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag: " + data.position);
        if (noMoreDragging == false)
        {
            data.pointerDrag = this.gameObject;

        }
        else
        {
            data.pointerDrag = null;
        }

        // child.position = new Vector3(child.position.x, child.position.y, 0f);

    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging )
        {
            transform.position = data.position;

        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        bool one = false;
        if(Vector2.Distance(transform.position, child1.position) < 10f)
        {
            one = true;
        }
        bool two = false;
        if(Vector2.Distance(transform.position, child2.position) < 10f) 
        {
            two = true;
            if(one == true)
            {
                if (Vector2.Distance(transform.position, child2.position) < Vector2.Distance(transform.position, child1.position))
                {
                    one = false;
                    Debug.Log("one is false");
                }
                else
                {
                    two = false;
                    Debug.Log("two is false");

                }
            }
        }
        if (one)
        {
            endDragStats(child1sprite);
        }
        else if(two)
        {
            endDragStats(child2sprite);            
        }
        data.pointerDrag = null;
    }
    void endDragStats(Sprite s)
    {
        transform.parent.GetChild(0).GetComponent<Image>().sprite = s;
        RectTransform rt = transform.parent.GetChild(0).gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(1200, 400);
        rt.localPosition = new Vector3(215, 0, 0);
        transform.GetChild(0).GetComponent<Image>().enabled = false;

        text.text = "Done";

        noMoreDragging = true;
        isDone = true;
    }
}
