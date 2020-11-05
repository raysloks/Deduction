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

    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();

        if (ShortTask == true)
        {
            pretextText.text = "Password is: ";
            GameController game = FindObjectOfType<GameController>();
            finalPass = PasswordSpawner.GetPassword("Passwords.txt", game.rng.NextULong(), game.rng.NextULong());
            password.text = finalPass;
        }
        else
        {
            pretextText.text = "Password is Located at:";
            PasswordSpawner passwordSpawner = FindObjectOfType<PasswordSpawner>();
            password.text = passwordSpawner.where;
            finalPass = passwordSpawner.pass;
            Debug.Log(finalPass + password.text);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //string actualPassword = password.text.Split(' ');
            if (finalPass.Replace(" ", string.Empty) == myText.text.Replace(" ", string.Empty))
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

}
