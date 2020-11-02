using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightTarget : MonoBehaviour
{
    public GameObject deathParticle;
    public GameObject textObject;


    void OnMouseDown()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Instantiate(textObject, transform.position, Quaternion.identity);
        DestroyImmediate(gameObject, true);

        // Destroy the gameObject after clicking on it
        Debug.Log("Right");
    }
}
