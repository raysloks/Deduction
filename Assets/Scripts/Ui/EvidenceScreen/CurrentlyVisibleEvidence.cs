using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class CurrentlyVisibleEvidence : MonoBehaviour
{
 
    public float speed = 10f;
    public Texture texture;
    private RawImage ri;

    //Arrow stuff
    public GameObject vis;
    public LayerMask arrowLayer;

    //motion sensor stuff
    public GameObject motionSensorEvidence;

    private Vector3 arrowOrginalPos;
    private Vector2 center;
    private Queue qt = new Queue();
    private RaycastHit2D hit;
    private bool moving = false;
    private bool firstMove = false;

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
        if (qt.Count > 0 && !moving)
        {
            moveArrow((Vector2)qt.Dequeue());
        }
    }

    //Move the arrow
    IEnumerator moveArrow(Vector2 target)
    {
        bool notHit = true;
        moving = true;
        Vector3 axis;
        if (target.y < vis.transform.position.y)
        {
            axis = new Vector3(0, 0, -1); // go down
        }
        else if (target.y > vis.transform.position.y)
        {
            axis = new Vector3(0, 0, 1); // go up
        }
        else
        {
            axis = new Vector3(0, 0, 1);
        }

        hit = Physics2D.Linecast(target, center, arrowLayer);

        while (notHit)
        {
            hit = Physics2D.Linecast(target, center, arrowLayer);
            if (hit.collider != null)
            {
                notHit = false;
            }
            else
            {
                vis.transform.RotateAround(center, axis, Time.deltaTime * speed);
                vis.transform.rotation = Quaternion.LookRotation(Vector3.forward, center);
            }

            yield return null;
        }

        moving = false;
        if (firstMove == false)
        {
            firstMove = true;
            vis.GetComponent<Image>().enabled = true;
        }
    }

    //Presents the evidence under the main meeting screen when you hover over a vote button
    public void ShowEvidence(EventCallbacks.Event eventInfo)
    {

        if (eventInfo is SendEvidenceEvent see)
        {
            if (see.Evidence == 2)
            {
                Debug.Log("Show Evidence: MotionSensor");
                ri.enabled = false;
                motionSensorEvidence.SetActive(true);
                motionSensorEvidence.GetComponent<ShowMotionSensorList>().addAllOptions(see.MotionSensorEvidence);
            }
            else if (see.Evidence == 1)
            {
                motionSensorEvidence.SetActive(false);
                ri.enabled = true;
                ri.texture = see.gc.screenshotHandler.photos[(ulong)see.photoIndex].texture;
            }
            else if (see.Evidence == 0)
            {
                ri.enabled = true;
                motionSensorEvidence.SetActive(false);
                ri.texture = texture;
            }

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

    //End of meeting cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main || pc.previous == GamePhase.EndOfMeeting)
        {

            motionSensorEvidence.SetActive(false);
            vis.transform.position = arrowOrginalPos;
            vis.GetComponent<Image>().enabled = false;
            ri.enabled = true;
            moving = false;
            firstMove = false;
            qt.Clear();
            ri.texture = texture;
        }
    }


}
