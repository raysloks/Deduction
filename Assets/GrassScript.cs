using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{
    List<playerObject> Entered = new List<playerObject>();
    List<grassObject> Grass = new List<grassObject>();
    List<grassObject> grassyMaterial = new List<grassObject>();

    List<grassObject> deleteMaterial = new List<grassObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in this.transform)
        {
            Grass.Add(new grassObject(t, t.GetComponent<SpriteRenderer>().material));

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
                Debug.Log("Delete");
                grassyMaterial.Remove(go);
            }
            deleteMaterial.Clear();
        }

        if (grassyMaterial.Count > 0)
        {
            foreach(grassObject go in grassyMaterial)
            {
                go.timer += Time.deltaTime;

                go.m.SetFloat("_GrassSpeed", 2f /  (go.timer));
                go.m.SetFloat("_GrassWind", 2f / (go.timer));
                if (go.timer > 2f)
                {

                    go.m.SetFloat("_GrassSpeed", 2f);
                    go.m.SetFloat("_GrassWind", 2f);
                    Debug.Log("reset");
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
            if(Vector2.Distance(grassy.t.position, go.position) < 0.25f)
            {
                if(grassyMaterial.Contains(grassy) == false)
                {

                    Debug.Log("Check");
                    grassyMaterial.Add(grassy);
                }
                // grassy.m
            }
        }
            
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Entered.Add(new playerObject(col.gameObject, (Vector2)col.transform.position));
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
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
        public Material m;
        public Transform t;
        private bool waving = false;
        public float timer = 0f;

        public grassObject(Transform t, Material m)
        {
            this.m = m;
            this.t = t;
        }

        
    }
}
