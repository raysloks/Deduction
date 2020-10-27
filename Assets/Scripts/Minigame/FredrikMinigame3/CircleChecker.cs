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
    public GameObject bg;
    public TextMeshPro text;

    private bool gameStarted = false;
    private bool PlayerInside = false;
    private bool isDone = false;

    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    private Vector2 target = Vector2.zero;
    private Vector3 startPos;
    private SpriteRenderer sr;
    //First get the centerCoord of Circle Collider in real world coords (the script is called in the gameobject that got the circlecollider)
    private CircleCollider2D circle;
    private Vector2 centerPos;
    //find a random point inside the aim circleCollider
  //  private Vector2 randVector = pointInsideCircle(centerPos);

    //then put the GameObject1 inside the CicleCollider of GameObject2 at random point
    //must create a Vector because c# interprets the ball.pos as a copy  :/



    // Start is called before the first frame update
    void Start()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.parent.position = player.transform.position;
        circle = bg.GetComponent<CircleCollider2D>();
        centerPos = findCircleCenter(transform.position);
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
                    target = pointInsideCircle(centerPos);
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
            target = pointInsideCircle(centerPos);
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

    
 
    Vector2 findCircleCenter(Vector3 aimTransform)
    {

        Vector2 aimPos = aimTransform;
        aimPos.x = aimTransform.x;
        aimPos.y = aimTransform.y;
        return (aimPos + circle.offset);

    }



    //return point inside the Collision circle
    Vector2 pointInsideCircle(Vector2 circlePos)
    {
        Vector2 newPoint;
        float angle = Random.Range(0.0F, 1.0F) * (Mathf.PI * 2);
        float radius = Random.Range(0.0F, 1.0F) * circle.radius;
        newPoint.x = circlePos.x + radius * Mathf.Cos(angle);
        newPoint.y = circlePos.y + radius * Mathf.Sin(angle);

        return (newPoint);
    }
}
