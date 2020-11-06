﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using EventCallbacks;

public class PasswordChecker : MonoBehaviour
{
    private string finalPass;
    private TextMeshProUGUI myText;
    public TextMeshProUGUI doneText;
    public TextMeshProUGUI password;
    public TextMeshProUGUI pretextText;
    public bool ShortTask = true;
    public List<AudioClip> wrongSounds;

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
            if (StringComparison(finalPass, myText.text.Trim()) == true)
            {
                doneText.text = "Done";
                Debug.Log("Done");
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
            else
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SoundEvent se = new SoundEvent();
                se.UnitSound = wrongSounds;
                se.UnitGameObjectPos = worldPosition;
                EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
                doneText.text = "Wrong Password";
                myText.text = "";
                Debug.Log(finalPass + " " + myText.text.Trim());
            }
        }
    }

    public static bool StringComparison(string s1, string s2)
    {
        if (s1.Length != (s2.Length - 1))
        {
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

}
