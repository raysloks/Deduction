using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordSpawner : MonoBehaviour
{

    GameObject passwordGameObject;
    int myChildCount;
    [HideInInspector] public string pass;
    [HideInInspector] public string where;
    // Start is called before the first frame update
    void Start()
    {
        
        myChildCount = transform.childCount;
        /*
        int r = Random.Range(0, (transform.childCount - 1));
        passwordGameObject = transform.GetChild(r).gameObject;
        Debug.Log(passwordGameObject.name + (transform.childCount - 1));
        passwordGameObject.SetActive(true);
        */
    }

    public void SetPassword(int set, string password)
    {
        for(int i = 0; i < myChildCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if( set <= myChildCount - 1)
        {
            passwordGameObject = transform.GetChild(set).gameObject;
            passwordGameObject.SetActive(true);
            pass = password;
            where = passwordGameObject.name;
            passwordGameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = "Password is: " + pass;
            // passwordGameObject.name = password;
        }
        
    }

}
