using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class IgnoreTab : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<InputField>().onValidateInput += Validate;
    }

    private char Validate(string input, int charIndex, char addedChar)
    {
        if (addedChar == 9)
            return (char)0;
        else if(addedChar == ';')
        {
            return ':';
        }
        return addedChar;
    }
}
