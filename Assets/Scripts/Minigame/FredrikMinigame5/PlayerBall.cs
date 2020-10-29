using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D rd;
    private bool inAir = false;
    private Vector3 startPos;
    private int jumps = 0;
    private int TotalJumps = 10;
    private bool isDone = false;
    public TextMeshPro text;
    public GameObject jumpRope;
    private BoxCollider2D collider;
    private bool jumpDone = false;

    void Awake()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        transform.parent.position = Player.transform.position;
        //transform.position = Player.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        text.text = jumps + "/" + TotalJumps;
        collider = jumpRope.GetComponent<BoxCollider2D>();
        //jumpRope.GetComponent<JumpRope>()
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inAir && !isDone)
        {
            rd.AddForce(Vector2.up * 350);
            inAir = true;
        }
        if (inAir && !isDone)
        {
           // rd.AddForce(-Vector2.up * 10);
           rd.velocity = (rd.velocity - Vector2.up * (17 *Time.deltaTime));
           if (transform.position.y < startPos.y)
           {
                if (jumpDone)
                {
                    jumps++;

                    //Debug.Log("stop");
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall") && inAir == true)
        {
            //Debug.Log("hit");
            jumpDone = true;
        }
        if (col.gameObject.CompareTag("Rope") && isDone == false)
        {
            if (jumps > 0)
            {
                jumps--;
                text.text = jumps + "/" + TotalJumps;
            }

            jumpDone = false;
            jumpRope.GetComponent<JumpRope>().speed = (jumpRope.GetComponent<JumpRope>().originalSpeed / 5);
            jumpRope.GetComponent<JumpRope>().GotHit();
            //Debug.Log("hi");
        }
    }

}
