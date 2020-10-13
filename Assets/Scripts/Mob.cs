using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;

    public bool IsAlive => type == 0;

    public ulong type;

    public Sprite[] sprites;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetType(ulong type)
    {
        this.type = type;
        if (type < (ulong)sprites.Length)
            sprite.sprite = sprites[type];
    }
}
