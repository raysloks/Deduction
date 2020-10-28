using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMovement : MonoBehaviour
{
    private Rigidbody2D rd;
    public float thrust = 100f;
    private float r;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        r = Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("POWA");
            Vector2 force = new Vector2(r,-transform.up.y);
            
            rd.AddForce(force * thrust);
        }
    }

    public void ThrustPaper()
    {
        //Debug.Log("POWA");
        Vector2 force = new Vector2(r, -transform.up.y);

        rd.AddForce(force * thrust);
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "PaperFile")
        {
            rd.velocity = Vector2.zero;
            //Debug.Log("Enter");
            rd.AddForce(Vector2.zero);
        }
    }
}
