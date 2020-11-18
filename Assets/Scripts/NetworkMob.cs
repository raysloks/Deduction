using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;

public class NetworkMob : Mob
{
    public struct Snapshot
    {
        public Vector3 position;
        public long time;
    }

    public List<Snapshot> snapshots = new List<Snapshot>();

    public long time;

    private new void Update()
    {
        snapshots.RemoveAll((snapshot) => snapshot.time < time - 150000000);

        Snapshot lower = snapshots.FindLast((snapshot) => snapshot.time < time);
        Snapshot upper = snapshots.Find((snapshot) => snapshot.time >= time);

        if (lower.time == 0 || upper.time == 0 || lower.time == upper.time)
            return;

        Vector3 diff = upper.position - lower.position;
        if (diff.x > 0f)
            characterTransform.localScale = new Vector3(-1f, 1f, 1f);
        if (diff.x < 0f)
            characterTransform.localScale = new Vector3(1f, 1f, 1f);

        animator.SetFloat("Speed", diff.magnitude / ((upper.time - lower.time) / 1000000000f));

        transform.position = Vector3.Lerp(lower.position, upper.position, (float)(time - lower.time) / (upper.time - lower.time));
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        time += (long)(Time.deltaTime * 1000000000);

        base.Update();
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
