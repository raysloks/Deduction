using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldUpperCase : MonoBehaviour
{
    public void MakeUpperCase(InputField inputField)
    {
        inputField.SetTextWithoutNotify(inputField.text.ToUpper());
    }
}
