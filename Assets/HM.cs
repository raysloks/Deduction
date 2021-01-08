using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM : MonoBehaviour
{
    Material m;
    bool alive = true;
    private float timer = -1f;
    public Material OrgMaterial;
    public GameObject parent;
    public Animation animation;
    // Start is called before the first frame update
    void Start()
    {
        m = this.GetComponent<SpriteRenderer>().material;
        m = OrgMaterial;
        Debug.Log("Set to 1");
        m.SetFloat("_OffsetUvX", 1f);

    }
    /*
    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            timer += Time.deltaTime;
            Debug.Log("Alive " + timer);
            m.SetFloat("_OffsetUvX", timer);
            if(timer >= 1f)
            {
                Debug.Log("reset");
                timer = -1f;
            }

        }
    }
    */

    public void Activate(bool set)
    {
        parent.SetActive(set);
    }

    public void SetAlive(bool aliveOrDead)
    {
        alive = aliveOrDead;
        if(alive = false)
        {
            animation.Stop();
            m.SetFloat("OffsetUvX", -1f);
        }
        else
        {
            animation.Play();
        }
    }
}
