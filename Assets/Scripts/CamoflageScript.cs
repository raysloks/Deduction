using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamoflageScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnCollisionEnter2D(Collision col)
    //{
    //    col.EnterCamo();
    //}

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("shit 1");

    }



    private void OnCollisionExit2D(Collision2D col)
    {


        Debug.Log("shit 2");

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("shit3");
    }
}
