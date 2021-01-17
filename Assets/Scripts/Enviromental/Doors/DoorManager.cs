using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour
{
    public Dictionary<ushort, Animator> doors = new Dictionary<ushort, Animator>();

    public void SetDoorState(ushort door, bool open)
    {
        if (doors.ContainsKey(door))
            doors[door].SetBool("Open", open);
    }

    public void OpenAllDoors()
    {
        foreach (var n in doors)
            n.Value.SetBool("Open", true);
    }
}
