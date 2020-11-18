using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public ushort index;

    private void Awake()
    {
        DoorManager doorManager = FindObjectOfType<DoorManager>();
        if (doorManager.doors.ContainsKey(index))
            FindObjectOfType<GameController>().CreateInfoPopup("Duplicate door index " + index + ".");
        else
            doorManager.doors.Add(index, GetComponent<Animator>());
    }
}
