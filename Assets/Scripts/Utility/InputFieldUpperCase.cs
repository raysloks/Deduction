﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldUpperCase : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<InputField>().onValidateInput += Validate;
    }

    private char Validate(string input, int charIndex, char addedChar)
    {
        if (addedChar == 0)
            return (char)0;
        return char.ToUpper(addedChar);
    }
}
