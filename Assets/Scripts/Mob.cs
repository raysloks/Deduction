using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Experimental.U2D.Animation;

public class Mob : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteResolver spriteResolver;
    public Transform characterTransform;

    public bool IsAlive => type == 0;

    public ulong type = 0;

    public ulong role = 0;

    public Sprite[] sprites;

    protected void Awake()
    {
    }

    public void SetType(ulong type)
    {
        this.type = type;
    }

    public void SetSprite(ulong sprite)
    {
        spriteResolver.SetCategoryAndLabel("Head", "Head " + (sprite + 1));
    }

    public void SetName(string name)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

}
