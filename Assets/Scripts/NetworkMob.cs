using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMob : MonoBehaviour
{
    public struct Snapshot
    {
        public Vector3 position;
        public long time;
    }

    public List<Snapshot> snapshots = new List<Snapshot>();

    public long time;

    private void Update()
    {
        snapshots.RemoveAll((snapshot) => snapshot.time < time - 150000000);

        Snapshot lower = snapshots.FindLast((snapshot) => snapshot.time < time);
        Snapshot upper = snapshots.Find((snapshot) => snapshot.time >= time);

        transform.position = Vector3.Lerp(lower.position, upper.position, (float)(time - lower.time) / (upper.time - lower.time));

        time += (long)(Time.deltaTime * 1000000000);
    }

    public void AddSnapshot(Snapshot snapshot)
    {
        long delta = time - snapshot.time;
        if (delta < -100000000)
            time = snapshot.time - 100000000;
        if (delta > -50000000)
            time = snapshot.time - 50000000;
        snapshots.Add(snapshot);
    }
}
