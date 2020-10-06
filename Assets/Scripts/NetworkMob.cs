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

    public bool isAlive = false;

    public Sprite[] sprites;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        snapshots.RemoveAll((snapshot) => snapshot.time < time - 150000000);

        Snapshot lower = snapshots.FindLast((snapshot) => snapshot.time < time);
        Snapshot upper = snapshots.Find((snapshot) => snapshot.time >= time);

        if (lower.time == 0 || upper.time == 0 || lower.time == upper.time)
            return;

        Vector3 diff = upper.position - lower.position;
        if (diff.x > 0f)
            sprite.flipX = true;
        if (diff.x < 0f)
            sprite.flipX = false;

        transform.position = Vector3.Lerp(lower.position, upper.position, (float)(time - lower.time) / (upper.time - lower.time));
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

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

    public void SetType(ulong type)
    {
        if(type == 0)
        {
            isAlive = true;
        }
        if (type < (ulong)sprites.Length)
            sprite.sprite = sprites[type];
        Debug.Log(type);
    }
}
