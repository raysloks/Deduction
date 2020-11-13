﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Experimental.U2D.Animation;

public class Mob : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteResolver spriteResolver;
    public Transform characterTransform;
    public Animator animator;
    public bool inCamo;

    public bool IsAlive => type == 0;

    public ulong type = 0;

    public ulong role = 0;

    protected void Awake()
    {
        inCamo = false;
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

    public virtual void EnterCamo()
    {
        inCamo = true;
        Hide();
    }

    public virtual void ExitCamo()
    {
        inCamo = false;
        Reveal();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreLayerCollision(0, 10, true);
        if (other.tag == "Camoflauge")
        {
            EnterCamo();
        }

    }

    private void OnTriggerExit2D()
    {
        ExitCamo();
    }

    public void Hide()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            spr[i].enabled = false;
        }
        GetComponentInChildren<TextMeshPro>().enabled = false;
    }

    public void Reveal()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            spr[i].enabled = true;
        }
        GetComponentInChildren<TextMeshPro>().enabled = true;
    }
}
