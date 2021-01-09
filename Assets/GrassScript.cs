using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{
    List<playerObject> Entered = new List<playerObject>();
    List<grassObject> Grass = new List<grassObject>();
    List<grassObject> grassyMaterial = new List<grassObject>();

    List<grassObject> deleteMaterial = new List<grassObject>();

    public float animationSpeed = 2f;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in this.transform) { 
            Animation anim = t.GetComponent<Animation>();
            Grass.Add(new grassObject(t, anim));
            foreach (AnimationState state in anim)
            {
                state.speed = animationSpeed;
            }
            t.GetComponent<SpriteRenderer>().material.SetFloat("_GrassSpeed", 2f);
            t.GetComponent<SpriteRenderer>().material.SetFloat("_GrassWind", 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Entered.Count > 0)
        {
            foreach(playerObject po in Entered)
            {
                po.timer += Time.deltaTime;
                if(po.timer > 0f)
                {

                    if ((Vector2)po.go.transform.position != po.pastPos)
                    {
                        SortListClosest(po.go.transform);
                    }
                    po.pastPos = (Vector2)po.go.transform.position;
                    po.timer = 0f;
                }
                

            }
        }

        if (deleteMaterial.Count > 0)
        {
            foreach (grassObject go in deleteMaterial)
            {
                grassyMaterial.Remove(go);
            }
            deleteMaterial.Clear();
        }

        if (grassyMaterial.Count > 0)
        {
            foreach(grassObject go in grassyMaterial)
            {
                go.timer += Time.deltaTime;
                go.a.Play();
                if (go.timer > 0.5f)
                {
                    go.timer = 0f;
                    deleteMaterial.Add(go);
                }
            }
        }
        
    }

    void SortListClosest(Transform go)
    {
        foreach (grassObject grassy in Grass)
        {
            if(Vector2.Distance(grassy.t.position, go.position) < range)
            {
                if(grassyMaterial.Contains(grassy) == false)
                {
                    grassyMaterial.Add(grassy);
                }
                // grassy.m
            }
        }
            
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" || col.tag == "Mob")
        {
            Entered.Add(new playerObject(col.gameObject, (Vector2)col.transform.position));
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Mob")
        {
            Entered.Remove(new playerObject(col.gameObject, (Vector2)col.transform.position));
        }
    }


    public class playerObject
    {
        public Vector2 pastPos;
        public GameObject go;
        public float timer = 0f;

        public playerObject(GameObject go, Vector2 pp)
        {
            this.pastPos = pp;
            this.go = go;
        }
    }

    public class grassObject
    {
        public Animation a;
        public Transform t;
        private bool waving = false;
        public float timer = 0f;

        public grassObject(Transform t, Animation a)
        {
            this.a = a;
            this.t = t;
        }

        
    }
}
