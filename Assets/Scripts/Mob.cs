using UnityEngine;
using System.Collections;
using TMPro;

public class Mob : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;

    public bool IsAlive => type == 0;

    public ulong type;

    public ulong role;

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

    public void SetName(string name)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
}
