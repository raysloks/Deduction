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
    public GameObject ghostFire;
    public GameObject shoes;
    public GameObject torsos;

    [HideInInspector] public bool inCamo;
    [HideInInspector] public bool inLocker;

    public bool IsAlive => type == 0;

    public ulong type = 0;

    public ulong role = 0;

    public ulong spriteIndex = 0;

    protected void Awake()
    {
        inCamo = false;
        inLocker = false;
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
        if (type == 2)
        {
            SetGhostAppearance();
        }
        else if (type == 0)
        {
            SetAliveAppearance();
        }

    }

    public void SetSprite(ulong sprite)
    {
        spriteIndex = sprite;
        if (type == 1)
            spriteResolver.SetCategoryAndLabel("Heads", "Head " + (sprite + 1) + " Dead");
        else
            spriteResolver.SetCategoryAndLabel("Heads", "Head " + (sprite + 1));
    }

    public void SetName(string name)
    {
        this.name = name;
        GetComponentInChildren<TextMeshPro>().text = name;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreLayerCollision(0, 10, true);
        if (other.CompareTag("Camouflage"))
        {
            EnterCamo();
        }
    }

    public void SetGhostAppearance()
    {
        ghostFire.SetActive(true);
        shoes.SetActive(false);
        torsos.SetActive(false);
    }

    public void SetAliveAppearance()
    {
        ghostFire.SetActive(false);
        shoes.SetActive(true);
        torsos.SetActive(true);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Camouflage"))
        {
            ExitCamo();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        //   int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        if (other.gameObject.tag == "Camouflage")
        {
            EnterCamo();
            Debug.Log("Collisons: " + other.gameObject.name);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        print("No longer in contact with " + other.transform.name);
    }

    public void SetTransparent()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 0.75f;
            spr[i].color = color;
        }
    }

    public void SetOpaque()
    {
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 1;
            spr[i].color = color;
        }
    }

    public virtual void EnterCamo()
    {
        inCamo = true;

        SetTransparent();

        if (GameObject.FindWithTag("Player").GetComponent<Player>().inCamo == false)
            Hide();
    }

    public virtual void ExitCamo()
    {
        inCamo = false;

        SetOpaque();

        Reveal();
    }

    public void Hide()
    {
        SpriteRenderer[] spr = characterTransform.GetComponentsInChildren<SpriteRenderer>();

        Debug.Log(spr.Length);

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
        SpriteRenderer[] spr = characterTransform.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            spr[i].enabled = true;
        }
        var tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
            tmp.enabled = true;
    }

    public void SetColor(float r, float g, float b)
    {
        SpriteRenderer[] spr = characterTransform.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color = new Color(r, g, b, 1.0f);
            spr[i].color = color;
        }
    }
}