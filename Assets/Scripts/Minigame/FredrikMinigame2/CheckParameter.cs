using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckParameter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            //Debug.Log("col");
        }
        //Debug.Log("col2 Par");
    }
}
