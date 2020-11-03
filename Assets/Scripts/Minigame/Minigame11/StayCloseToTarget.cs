using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayCloseToTarget : MonoBehaviour
{
    List<GameObject> mobs = new List<GameObject>();
    public float sliderGain = 0.1f;
    private Vector3 orignialScale;
    private bool isDone = false;
    private Transform smallbar;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        smallbar = transform.GetChild(0).gameObject.transform.GetChild(0);
        orignialScale = smallbar.localScale;
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mobs.Count > 0 && !isDone)
        {
            if (player.canMove == true)
            {
                smallbar.localScale += new Vector3((sliderGain * Time.deltaTime), 0f, 0f);
                if (smallbar.localScale.x >= 2f)
                {
                    Debug.Log("Done");
                    isDone = true;
                }
            }         
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Mob" && mobs.Contains(col.gameObject) == false)
        {
            Debug.Log("BOOYAH ENTER");
            mobs.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Mob" && mobs.Contains(col.gameObject) == true)
        {
            Debug.Log("BOOYAH EXIT");            
            mobs.Remove(col.gameObject);
            if (mobs.Count == 0 && isDone == false)
            {
                smallbar.localScale = orignialScale;
            }
        }
    }

    public bool getIsDone()
    {
        return isDone;
    }
}
