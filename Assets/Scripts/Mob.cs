using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Experimental.U2D.Animation;

public class Mob : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteResolver spriteResolver;
    public Transform characterTransform;
    public Animator animator;

    [HideInInspector] public bool inCamo;

    public bool IsAlive => type == 0;

    public ulong type = 0;

    public ulong role = 0;

    public ulong spriteIndex = 0;

    protected void Awake()
    {
        inCamo = false;
    }

    protected void Update()
    {
        UpdateFeetPos();
    }
    
    public void UpdateFeetPos()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector("_FeetPos", (Vector2)transform.position);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

    public void SetType(ulong type)
    {
        this.type = type;
    }

    public void SetSprite(ulong sprite)
    {
        spriteIndex = sprite;
        if (type == 1) spriteResolver.SetCategoryAndLabel("Heads", "Head " + (sprite + 1) + " Dead");
        else spriteResolver.SetCategoryAndLabel("Heads", "Head " + (sprite + 1));
    }

    public void SetName(string name)
    {
        this.name = name;
        GetComponentInChildren<TextMeshPro>().text = name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreLayerCollision(0, 10, true);
        if (other.CompareTag("Camouflage"))
        {
            EnterCamo();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Camouflage"))
        {
            ExitCamo();
        }
    }

    public virtual void EnterCamo()
    {
        inCamo = true;
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 0.75f;
            spr[i].color = color;
        }
        if (GameObject.FindWithTag("Player").GetComponent<Player>().inCamo == false)
            Hide();
    }

    public virtual void ExitCamo()
    {
        inCamo = false;
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 1;
            spr[i].color = color;
        }
        Reveal();
    }

    public void Hide()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            spr[i].enabled = false;
        }
        var tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
            tmp.enabled = false;
    }

    public void Reveal()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            spr[i].enabled = true;
        }
        var tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
            tmp.enabled = true;
    }
}
