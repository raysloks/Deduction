using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightTarget : MonoBehaviour
{
    public GameObject deathParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        DestroyImmediate(gameObject, true);

        // Destroy the gameObject after clicking on it
        Debug.Log("Right");
    }
}
