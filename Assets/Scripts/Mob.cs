using UnityEngine;
using System.Collections;
using TMPro;

public class Mob : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;

    public bool IsAlive => type == 0;

    public ulong type = 0;

    public ulong role = 0;

    public Sprite[] sprites;

    protected void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetType(ulong type)
    {
        this.type = type;
        if (type < (ulong)sprites.Length)
            sprite.sprite = sprites[type];
    }

    public void SetSprite(ulong sprite)
    {

    }

    public void SetName(string name)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

}
