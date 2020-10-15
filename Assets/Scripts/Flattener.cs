using UnityEngine;
using System.Collections;

public class Flattener : MonoBehaviour
{
    private void Update()
    {
        Vector3 position = transform.position;
        position.z = 0f;
        transform.position = position;
    }
}
