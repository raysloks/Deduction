using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRope : MonoBehaviour
{
    private LineRenderer Line;
    private GameObject target;
    private SpriteRenderer sr;
    private float speedIncrease = 20f;
    private float evenMoreSpeedIncrease = 2f;
    private ContactFilter2D contactFilter;
    private CircleCollider2D col2d;
    private bool currentlyHit = false;

    [HideInInspector] public float originalSpeed;
    [HideInInspector] public bool isDone = false;
    public float speed = 140f;
    public float noMoreThanSpeed = 300f;
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
        col2d = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        target = transform.parent.gameObject;
        axis = new Vector3(0, 0, 1);
        contactFilter.SetLayerMask(layerMask);
    }

    // Update is called once per frame
    // Rotate spike ball and control its speed
    public void Update()
    {
        Vector3 Target = target.transform.position;
        transform.RotateAround(Target, axis, speed * Time.deltaTime);
        if (speed < originalSpeed)
        {
            speed += (speedIncrease + evenMoreSpeedIncrease) * Time.deltaTime;

        }
        else if (noMoreThanSpeed > speed)
        {
            speed += speedIncrease * Time.deltaTime;
        }
    }

    public void GotHit()
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
