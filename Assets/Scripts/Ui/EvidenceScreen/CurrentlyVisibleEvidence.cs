using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class CurrentlyVisibleEvidence : MonoBehaviour
{
    public GameObject vis;
    private Vector3 arrowOrginalPos;
    public float speed = 10f;
    public Texture texture;
    private RawImage ri;
    Vector2 center;
    private bool moving = false;
    private bool firstMove = false;
    public LayerMask arrowLayer;
    Queue qt = new Queue();

    private RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        arrowOrginalPos = vis.transform.position;
        ri = GetComponent<RawImage>();
        EventSystem.Current.RegisterListener(EVENT_TYPE.SHOW_EVIDENCE, ShowEvidence);

        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }
    void Update()
    {
        if(qt.Count > 0 && !moving)
        {
            moveArrow((Vector2)qt.Dequeue());
        }
    }
    IEnumerator moveArrow(Vector2 target)
    {
        bool notHit = true;
        moving = true;
        Vector3 axis = new Vector3(0, 0, 1);
        hit = Physics2D.Linecast(target, center, arrowLayer);

        while (notHit)
        {
            hit = Physics2D.Linecast(target, center, arrowLayer);
            if(hit.collider != null)
            {
                notHit = false;
            }
            else
            {
                vis.transform.RotateAround(center, axis, Time.deltaTime * speed);
            }

            yield return null;
        }
        Debug.Log("End " + Vector2.Distance(target, vis.transform.position) + "X ABS " + Mathf.Abs(target.y - vis.transform.position.y) + "Y ABS " + Mathf.Abs(target.x - vis.transform.position.x));
        moving = false;
        if(firstMove == false)
        {
            firstMove = true;
            vis.GetComponent<Image>().enabled = true;
        }
    }
    public void ShowEvidence(EventCallbacks.Event eventInfo)
    {
        
        SendEvidenceEvent see = (SendEvidenceEvent)eventInfo;

        if (see.Evidence == 1)
        {

            Debug.Log("Show Evidence byteLenght " + see.byteArray.Length);
            Texture2D sampleTexture = new Texture2D(2, 2);

            bool isLoaded = sampleTexture.LoadImage(see.byteArray);

            ri.texture = sampleTexture;
            if (!moving)
            {
                StartCoroutine(moveArrow(see.positionOfTarget));
            }
            else
            {
                qt.Enqueue(see.positionOfTarget);
            }
        }else if(see.Evidence == 0)
        {
            ri.texture = texture;
            if (!moving)
            {
                StartCoroutine(moveArrow(see.positionOfTarget));
            }
            else
            {
                qt.Enqueue(see.positionOfTarget);
            }
        }

    }
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main || pc.previous == GamePhase.EndOfMeeting)
        {
            vis.transform.position = arrowOrginalPos;
            vis.GetComponent<Image>().enabled = false;
            moving = false;
            firstMove = false;
            qt.Clear();
            ri.texture = texture;
        }
    }


}
