using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class CurrentlyVisibleEvidence : MonoBehaviour
{
    public GameObject vis;
    public float speed = 10f;
    private RawImage ri;
    Vector2 center;
    private bool moving = false;
    int moveLater = 0;
    Queue qt = new Queue();

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector2(Screen.width / 2, Screen.height / 2);
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
        moving = true;
        Vector3 axis = new Vector3(0, 0, 1);
        while (Vector2.Distance(target, vis.transform.position) < 0.1f)
        {
            vis.transform.RotateAround(center, axis, Time.deltaTime * speed);
           
            yield return null;
        }
        Debug.Log("End");
        moving = false;
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
              //  StartCoroutine(moveArrow(see.positionOfTarget));
            }
            else
            {
              //  qt.Enqueue(see.positionOfTarget);
            }
        }

    }
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Main)
        {
            ri.texture = null;
        }
    }


}
