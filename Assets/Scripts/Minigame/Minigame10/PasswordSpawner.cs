using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PasswordSpawner : MonoBehaviour
{
    //Script for spawning random password location (Sticky note) for Password long task
    
    [HideInInspector] public string pass;
    [HideInInspector] public string where;

    public void SetPassword(ulong password, ulong suffix, ulong location)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        if (transform.childCount > 0)
        {
            location %= (ulong)transform.childCount;
            var child = transform.GetChild((int)location);
            child.gameObject.SetActive(true);
            pass = GetPassword("Passwords.txt", password, suffix);
            where = child.name;
            child.GetComponentsInChildren<TextMeshPro>()[1].text = "Password is: " + pass;
        }
    }

    public static string GetPassword(string fileName, ulong password, ulong suffix)
    {
        string result;
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        string fileContents = sr.ReadToEnd();
        sr.Close();
        result = fileContents;

        string[] lines = result.Split('\n');

        int r = (int)(password % (ulong)lines.Length);
        string str = lines[r].Trim();
        str += (suffix % 100).ToString().PadLeft(2, '0');
        return str;
    }

}
