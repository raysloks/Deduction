using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetSpawner : MonoBehaviour
{

    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    private Vector2 target = Vector2.zero;

    private bool isDone = false;

    public GameObject TargetPrefab;
    public GameObject WrongTargetPrefab;
    public TextMeshPro text;
    public GameObject bg;

    private GameObject currentPrefab;
    private float minimize = 60f;
    private int score = 0;
    private int targetScore = 10;
    private int hitBabyPenalty = 5;
    private bool GivePoints = true;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        isDone = false;
        GivePoints = true;

        transform.parent.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        minScreenBounds = bg.GetComponent<SpriteRenderer>().bounds.min;
        maxScreenBounds = bg.GetComponent<SpriteRenderer>().bounds.max;
      //  Renderer.bounds
        Debug.Log("bg bounds min " + bg.GetComponent<SpriteRenderer>().sprite.bounds.min + " Screen Bounds " + Camera.main.ScreenToWorldPoint(new Vector3(minimize, minimize, 0)));
        Debug.Log("bg bounds max " + bg.GetComponent<SpriteRenderer>().sprite.bounds.max + " Screen Bounds " + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - minimize, Screen.height - minimize, 0)));
        //minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(minimize, minimize, 0));
        //maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - minimize, Screen.height - minimize, 0));
        target = RandomPointInScreenBounds();
        text.text = score + "/" + targetScore;
        currentPrefab = Instantiate(TargetPrefab, target, Quaternion.identity);
        currentPrefab.transform.SetParent(transform);

        StartCoroutine(SpawnTarget(1));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator SpawnTarget(int Sec)
    {
        float counter = Sec;

        while (counter > 0)
        {
            if (currentPrefab == null)
            {
                Debug.Log("null;");
                if (GivePoints == true)
                {
                    score++;
                    text.text = score + "/" + targetScore;
                }
                else
                {
                    targetScore += hitBabyPenalty;
                    text.text = score + "/" + targetScore;

                }
                if (score >= targetScore)
                {
                    text.text = "Done";
                    isDone = true;
                    FindObjectOfType<MinigamePopupScript>().MinigameWon();
                }
                break;
            }
            //text.text = "Get In Circle " + Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null;
        }

        if (isDone == false)
        {
            if (currentPrefab != null)
            {
                DestroyImmediate(currentPrefab, true);
            }

            int r = (int)Random.Range(1f, 10f);
            if (r > 2)
            {
                currentPrefab = Instantiate(TargetPrefab, target, Quaternion.identity);
                GivePoints = true;
            }
            else
            {
                GivePoints = false;
                currentPrefab = Instantiate(WrongTargetPrefab, target, Quaternion.identity);
            }
            currentPrefab.transform.SetParent(transform);
            target = RandomPointInScreenBounds();
            StartCoroutine(SpawnTarget(1));
        }
    }

    public Vector2 RandomPointInScreenBounds()
    {
        return new Vector2(
            Random.Range(minScreenBounds.x, maxScreenBounds.x),
            Random.Range(minScreenBounds.y , maxScreenBounds.y)
        );
    }
}
