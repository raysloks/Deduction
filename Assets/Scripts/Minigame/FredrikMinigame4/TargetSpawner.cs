using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    private Vector2 target = Vector2.zero;

    private bool isDone = false;

    public GameObject TargetPrefab;
    public GameObject WrongTargetPrefab;

    private GameObject currentPrefab;


    // Start is called before the first frame update
    void Start()
    {
        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        target = RandomPointInScreenBounds();
        currentPrefab = TargetPrefab;
        Instantiate(currentPrefab, target);
        StartCourutine(SpawnTarget(2));

    }

    // Update is called once per frame
    void Update()
    {
       

    }

    IEnumerator SpawnTarget(int Sec)
    {

        float counter = Sec;

        
        while (counter > 1)
        {
           // text.text = "Get In Circle " + Mathf.Round(counter).ToString();
            counter -= Time.deltaTime;
            yield return null;
        }
        Destroy(currentPrefab);
        currentPrefab = TargetPrefab;
        Instantiate(currentPrefab, target);
        target = RandomPointInScreenBounds();
        StartCourutine(SpawnTarget(2));
    }

        public Vector2 RandomPointInScreenBounds()
    {
        return new Vector2(
            Random.Range(minScreenBounds.x, maxScreenBounds.x),
            Random.Range(minScreenBounds.y, maxScreenBounds.y)
        );
    }
}
