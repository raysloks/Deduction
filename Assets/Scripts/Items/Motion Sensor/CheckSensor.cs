using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EventCallbacks;

public class CheckSensor : MonoBehaviour
{
    private int enter = 0;
    private List<string> peopleEntered = new List<string>();
    private List<int> secondsIn = new List<int>();
    [HideInInspector] public GameController gc;
    [HideInInspector] public EvidenceHandler evidenceHandler;

    [HideInInspector] public List<Vector2> fingerPositions;

    public LayerMask lm;

    public float lineWidth = 0.15f;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<GameObject> goWaitList = new List<GameObject>();
    private bool leftRight = false;
    public SpriteRenderer outline;
  
    // Start is called before the first frame update
    void Start()
    {
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_STARTED, meetingStarted);
        if(gc == null)
        {
            gc = GameObject.Find("GameController").GetComponent<GameController>();
        }
        RaycastHit2D hit1;
        RaycastHit2D hit2;
        if (gc.player.move.x == 0f && gc.player.move.y == 0f)
        {
            Debug.Log("ZERO");
            hit1 = Physics2D.Raycast(transform.position, Vector2.left, 1000f, lm);
            hit2 = Physics2D.Raycast(transform.position, Vector2.right, 1000f, lm);
            leftRight = true;
        }
        else if(Mathf.Abs(gc.player.move.y) > Mathf.Abs(gc.player.move.x))
        {
            Debug.Log("LEFTRIGHT");
            hit1 = Physics2D.Raycast(transform.position, Vector2.left, 1000f, lm);
            hit2 = Physics2D.Raycast(transform.position, Vector2.right, 1000f, lm);
            leftRight = true;
        }
        else
        {
            Debug.Log("UP");
            hit1 = Physics2D.Raycast(transform.position, -Vector2.up, 1000f, lm);
            hit2 = Physics2D.Raycast(transform.position, Vector2.up, 1000f, lm);
        }
        if (hit1.collider != null && hit2.collider != null)
        {
            CreateLine(hit1.point, hit2.point);
        }
        else
        {
            Debug.Log("Nothing hit");
        }
    }


    void CreateLine(Vector2 hit1, Vector2 hit2)
    {
        //Instantiate currentLine from line prefab and get its renderer/collider
        lineRenderer = GetComponent<LineRenderer>();

        //set Line Width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        //clear all mouse/finger positions and add current pos
        fingerPositions.Clear();
        fingerPositions.Add(hit1);
        fingerPositions.Add(hit2);

        //Set line pos to current mouse/finger pos
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);

        BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        Vector2 v2;
        if (leftRight)
        {
            v2 = new Vector2(collider.size.x - 0.5f, collider.size.y / 5);
        }
        else
        {
            v2 = new Vector2(collider.size.x / 5, collider.size.y - 0.5f);
        }
        collider.size = v2;

        StartCoroutine(FadeOutOutline(1f));


    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Mob")
        {
            StartCoroutine(FadeInOutline2(0.2f));
            enter++;
            peopleEntered.Add(col.gameObject.name);
            int timer = (int)gc.roundTimer;
            secondsIn.Add(timer);
            Debug.Log("Enter Col: " + col.gameObject.name + " Number entered: " + enter+ " Seconds In round : " + timer);
        }
        if(col.tag == "Player")
        {
            StartCoroutine(FadeInOutline2(0.2f));
            Debug.Log("Enter");
        }
    }

    public void meetingStarted(EventCallbacks.Event eventinfo)
    {
        evidenceHandler.AddSensorList(peopleEntered, secondsIn);
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0.2f)
        {
            Color color = lineRenderer.startColor;
            color.a = counter / seconds;

            Color color1 = lineRenderer.endColor;
            color1.a = counter / seconds;

            lineRenderer.SetColors(color, color1);
            counter -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeInOutline2(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOutOutline2(0.2f));

    }

    IEnumerator FadeOutOutline2(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter -= Time.deltaTime;
            yield return null;
        }
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Discussion)
        {
         //   evidenceHandler.AddSensorList(peopleEntered);
        }

        if(pc.previous == GamePhase.EndOfMeeting || pc.phase == GamePhase.Setup)
        {
            Destroy(this.gameObject);
        }
    }
}
