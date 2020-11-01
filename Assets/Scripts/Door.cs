using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public ushort index;

    private void Awake()
    {
        FindObjectOfType<DoorManager>().doors.Add(index, GetComponent<Animator>());
    }
}
