using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRope : MonoBehaviour
{
    private LineRenderer Line;
    private GameObject target;
    private SpriteRenderer sr;
    [HideInInspector]public float speed = 140f;
    [HideInInspector] public float originalSpeed;
    [HideInInspector] public bool isDone = false;
    private float noMoreThanSpeed = 300f;
    private float speedIncrease = 20f;
    private float evenMoreSpeedIncrease = 2f;
   
    private ContactFilter2D contactFilter;
    private CircleCollider2D col2d;
    public LayerMask layerMask;
    Vector3 axis;

    void Awake()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        transform.parent.position = Player.transform.position; 
    }
    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
        Line = GetComponent<LineRenderer>();
        Line.positionCount = 2;
        col2d = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        target = transform.parent.gameObject;
        axis = new Vector3(0, 0, 1);
        contactFilter.SetLayerMask(layerMask);
       // transform.RotateAround(point, axis, Time.deltaTime * 10);
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 Target = target.transform.position;
        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, Target);
        transform.RotateAround(Target, axis, speed * Time.deltaTime);
        if(speed < originalSpeed)
        {
            Debug.Log("less than orignal");
            speed += (speedIncrease + evenMoreSpeedIncrease) * Time.deltaTime;
            
        }else if(noMoreThanSpeed > speed)
        {
            Debug.Log("less than noMoreThanSpeed");

            speed += speedIncrease * Time.deltaTime;
        }
        // Debug.Log(speed);
    }

    public void GotHit()
    {
        
        StartCoroutine(Hit(1));
    }

    IEnumerator Hit(int Sec)
    {

        float counter = Sec;


        while (counter > (Sec/2))
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
    }


 }
