using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Spawner : MonoBehaviour
{
    public float spawnRate = 4f;
    private float spawnAmount = 0f;
    public float spawnTotal = 70f;
    public GameObject prefab;
    private float timer = 0;
    private GameObject newObj;
    public List<Sprite> s = new List<Sprite>();
    private List<GameObject> gos = new List<GameObject>();
    private bool thrust = false;
    private int filed = 0;
    public int checkWin = 9;
    private bool taskCompleted = false;

    private string weapon1;
    private string weapon2;
    private TextMeshProUGUI text;
    private Transform[] ts;



    // Start is called before the first frame update
    void Start()
    {
        SetWeaponText("Weapon.txt");
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        text.text = filed.ToString() + " / " + checkWin;
    }

    // Update is called once per frame
    void Update()
    {
        if(thrust == true)
        {
            newObj.GetComponent<FileMovement>().ThrustPaper();
            thrust = false;
        }
        if (spawnAmount < spawnTotal && taskCompleted == false)
        {
            timer += Time.deltaTime;
        }
        if (timer > spawnRate && taskCompleted == false && transform.childCount < 7f)
        {
            spawnAmount++;
            newObj = Instantiate(prefab, transform);
            newObj.transform.position = transform.position;
            int r = Random.Range(0, (s.Count - 1));
            newObj.GetComponent<Image>().sprite = s[r];
            string name = readTextFile("Adjektiv.txt");
            name += "\n" + readTextFile("Substantiv.txt");
            System.Random rnd = new System.Random();
            if (rnd.Next(2) == 0)
            {
                name += "\n" + weapon1;
                newObj.name = weapon1;
                newObj.GetComponent<DragDrop>().RightOrLeft("Right");

            }
            else
            {
                name += "\n" + weapon2;
                newObj.name = weapon2;
                newObj.GetComponent<DragDrop>().RightOrLeft("Left");
            }

            newObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = name;
            timer = 0;
            thrust = true;
        }else if(timer > spawnRate && taskCompleted == false)
        {
           
            ts = GetComponentsInChildren<Transform>();
            float longestDistance = 0f;
            foreach (Transform t in ts)
            {
                if(Vector2.Distance(t.position, transform.position) > 900f)
                {
                    Debug.Log("Removed" + Vector2.Distance(t.position, transform.position));
                    Destroy(t.gameObject);
                }
                else
                {
                    if(longestDistance < Vector2.Distance(t.position, transform.position))
                    {
                        longestDistance = Vector2.Distance(t.position, transform.position);
                    }
                }
            }
            Debug.Log(longestDistance);
            timer = 1f;

        }
    }

    public void CheckWinCondition()
    {
        if(taskCompleted != true)
        {
            filed++;
            text.text = filed.ToString() + " / " + checkWin;
        }
       
        if (filed >= checkWin)
        {
            text.text = "Done";

            taskCompleted = true;
            FindObjectOfType<MinigamePopupScript>().MinigameWon();
        }
    }

    private string readTextFile(string fileName)
    {
        string result;
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();
        result = fileContents;

        string[] lines = result.Split("\n"[0]);

        int r = Random.Range(0, lines.Length);
        return lines[r];
      

    }

    private void SetWeaponText(string fileName)
    {
        string result;
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();
        result = fileContents;

        string[] lines = result.Split("\n"[0]);

        int r = Random.Range(0, lines.Length);

        weapon1 = lines[r];
        transform.parent.GetChild(0).gameObject.name = weapon1;
        transform.parent.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = weapon1;
        int ra = Random.Range(0, lines.Length);

        while (ra == r)
        {
            ra = Random.Range(0, lines.Length);
        }
        weapon2 = lines[ra];
        transform.parent.GetChild(1).gameObject.name = weapon2;
        transform.parent.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = weapon2;



    }
}
