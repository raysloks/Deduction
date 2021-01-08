using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensor
{
    public int player;
    public int totalRoundTime;
    public List<string> names = new List<string>();
    public List<int> secondsIn = new List<int>();

    public List<ulong> playerIds = new List<ulong>();
    public List<Sprite> playerSprites = new List<Sprite>();
    public int number;

}
