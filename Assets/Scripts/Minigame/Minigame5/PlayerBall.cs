using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EventCallbacks;

public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D rd;
    private Vector3 startPos;
    private int jumps = 0;
    private int TotalJumps = 10;
    private BoxCollider2D collider;
    private SpriteRenderer sr;

    private bool inAir = false;
    private bool isDone = false;
    private bool jumpDone = false;
    private bool currentlyHit = false;

    public TextMeshPro text;
    public GameObject jumpRope;
    public float speed = 350f;

    public List<AudioClip> wrongSounds;
    public List<AudioClip> correctSounds;

    void Awake()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        transform.parent.position = Player.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        text.text = jumps + "/" + TotalJumps;
        collider = jumpRope.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inAir && !isDone)
        {
            rd.AddForce(Vector2.up * speed);
            inAir = true;
        }
        if (inAir && !isDone)
        {
            Debug.Log(Vector2.Distance(transform.parent.position, transform.position));
            if(Vector2.Distance(transform.parent.position, transform.position) < 0.01f)
            {
                rd.velocity = Vector2.zero;
            }
           rd.velocity = (rd.velocity - Vector2.up * (17 *Time.deltaTime));
           if (transform.position.y < startPos.y)
           {
                if (jumpDone)
                {
                    jumps++;

                    SoundEvent se = new SoundEvent();
                    se.UnitSound = correctSounds;
                    se.UnitGameObjectPos = transform.position;
                    EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);

                    if (jumps >= TotalJumps)
                    {
                        text.text = "Done";
                        isDone = true;
                        FindObjectOfType<MinigamePopupScript>().MinigameWon();
                        jumpRope.GetComponent<JumpRope>().speed = 0f;
                        jumpRope.GetComponent<JumpRope>().enabled = false;
                    }
                    else
                    {
                        text.text = jumps + "/" + TotalJumps;
                    }
                    jumpDone = false;
                }
               
                transform.position = startPos;
                rd.velocity = Vector2.zero;
                inAir = false;
           }
        }
    }

    //Check if you hit spike ball or if you actually complete a jump over the spike ball
    void OnTriggerEnter2D(Collider2D col)
    {
        //Completed Jump
        if (col.CompareTag("Wall") && inAir == true)
        {
            jumpDone = true;
        }
        //Hit spike ball
        if (col.gameObject.CompareTag("Rope") && isDone == false)
        {
            if (jumps > 0)
            {
                jumps--;
                text.text = jumps + "/" + TotalJumps;
            }

            SoundEvent se = new SoundEvent();
            se.UnitSound = wrongSounds;
            se.UnitGameObjectPos = transform.position;
            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);

            jumpDone = false;
            jumpRope.GetComponent<JumpRope>().speed = (jumpRope.GetComponent<JumpRope>().originalSpeed / 5);
            jumpRope.GetComponent<JumpRope>().GotHit();
            thisGotHit();
        }
    }

    public void thisGotHit()
    {
        if (!currentlyHit)
        {
            currentlyHit = true;
            StartCoroutine(Hit(1));
        }

    }

    IEnumerator Hit(int Sec)
    {
        float counter = Sec;

        while (counter > (Sec / 2))
        {
            sr.color = Color.Lerp(Color.white, Color.red, counter);
            counter -= Time.deltaTime;
            yield return null;
        }

        while (counter > 0)
        {
            sr.color = Color.Lerp(Color.red, Color.white, counter);
            counter -= Time.deltaTime;
            yield return null;
        }
        currentlyHit = false;
    }

}
