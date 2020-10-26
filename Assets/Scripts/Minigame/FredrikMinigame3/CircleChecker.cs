using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CircleChecker : MonoBehaviour
{

    public float step = 0.001f;
    public int SecondsToStart;
    public int SecondsToEnd;
    public TextMeshPro text;

    private bool gameStarted = false;
    private bool PlayerInside = false;
    private bool isDone = false;

    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    private Vector2 target = Vector2.zero;
    private Vector3 startPos;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(StartIn(SecondsToStart));
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
       if(gameStarted)
       {
            transform.position = Vector2.MoveTowards(transform.position, target, step * Time.deltaTime);
            if (Vector2.Distance(transform.position, target) < 1.5f)
            {
                sr.color = Color.Lerp(Color.white, Color.red, Vector2.Distance(transform.position, target));
                if (Vector2.Distance(transform.position, target) < 0.01f)
                {
                    target = RandomPointInScreenBounds();
                }
                // StartCoroutine(StartIn(SecondsToStart));
            }
            
       }
    }
    
    IEnumerator StartIn(int Sec)
    {
        float counter = Sec;


        while (counter > 1)
        {
            text.text = "Get In Circle " + Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null; 
        }

        text.text = " ";
        if(PlayerInside != true)
        {
            StartCoroutine(StartIn(SecondsToStart));
        }
        else
        {
            gameStarted = true;
            target = RandomPointInScreenBounds();
            StartCoroutine(EndIn(SecondsToEnd));

        }
    }

    IEnumerator EndIn(int Sec)
    {
        float counter = Sec;

        while (counter > 1 && gameStarted)
        {
            text.text = Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null;
        }
        if (gameStarted)
        {
            text.text = "Done";
            isDone = true;
            target = Vector2.zero;
            gameStarted = false;
            FindObjectOfType<MinigamePopupScript>().MinigameWon();
        }

    }

    public Vector2 RandomPointInScreenBounds()
    {
        return new Vector2(
            Random.Range(minScreenBounds.x, maxScreenBounds.x),
            Random.Range(minScreenBounds.y, maxScreenBounds.y)
        );
    }



    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ball" && gameStarted)
        {
            PlayerInside = false;
            transform.position = startPos;
            gameStarted = false;
            sr.color = Color.white;
            StartCoroutine(StartIn(SecondsToStart));
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball" )
        {
            PlayerInside = true;
        }
    }
}
