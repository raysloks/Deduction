using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerScript : MonoBehaviour
{
    public bool occupied;
    private GameObject occupant;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        occupied = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttemptToHide(GameObject body)
    {
        if (!occupied)
            HidePerson(body);
        else
            RemovePerson();
    }

    private void HidePerson(GameObject body)
    {
        occupant = body;
        occupant.transform.position = this.transform.position;

    }

    private void RemovePerson()
    {
        occupant.transform.position = this.transform.position + new Vector3(0f, -0.5f, 0f);
        occupant = null;

    }


}
