using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PasswordChecker : MonoBehaviour
{
    private string finalPass;
    private TextMeshProUGUI myText;
    public TextMeshProUGUI doneText;
    public TextMeshProUGUI password;
    public TextMeshProUGUI pretextText;
    public bool ShortTask = true;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        
        if (ShortTask == true)
        {           
            finalPass = readTextFile("Passwords.txt");
            pretextText.text = "Password is: ";
            for(int i = 0; i < 4; i++)
            {
                int r = Random.Range(0, 9);
                finalPass += r.ToString();
            }
            password.text = finalPass;
        }
        else
        {
            pretextText.text = "Password is Located at:";
            //ugly but works
            PasswordSpawner passhold = transform.parent.parent.parent.parent.gameObject.GetComponent<MinigameHolder>().noteLocation.GetComponent<PasswordSpawner>();
            password.text = passhold.where;
            finalPass = passhold.pass;
            Debug.Log(finalPass + password.text);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //string actualPassword = password.text.Split(' ');
            if (StringComparison(finalPass.Replace(" ", string.Empty), myText.text.Replace(" ", string.Empty)) == true)
            {
                doneText.text = "Done";
                Debug.Log("Done");
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
            else
            {
                doneText.text = "Wrong Password";
                int count = finalPass.Split(' ').Length;
                int count2 = myText.text.Split(' ').Length;
                Debug.Log("Nope " + count + " " + finalPass.Split(' ')[finalPass.Split(' ').Length - 1] + " vs " + myText.text + " " + count2);
            }
        }
    }
    public static bool StringComparison(string s1, string s2)
    {
        if (s1.Length  != (s2.Length - 1)) {
            Debug.Log("Different Lenght " + s1.Length + " vs " + s2.Length);
            return false;
        }
        for (int i = 0; i < s1.Length - 1; i++)
        {
            if (s1[i] != s2[i])
            {
                Debug.Log("The " + i.ToString() + "th character is different. " + s1[i] + " vs " + s2[i]);
                return false;
            }
        }
        return true;
    }

    private string readTextFile(string fileName)
    {
        string result;
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();
        result = fileContents;

        string[] lines = result.Split("\n"[0]);

        int r = Random.Range(0, lines.Length - 1);
        string str = lines[r];
        if (str != null && str.Length > 0)
        {
            str = str.Substring(0, str.Length - 1);
        }
        return str;


    }

}
