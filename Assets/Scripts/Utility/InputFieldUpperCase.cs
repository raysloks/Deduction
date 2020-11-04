using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldUpperCase : MonoBehaviour
{
    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        inputField.text = value.ToUpper();
    }
}
