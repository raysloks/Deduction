using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinimapController : MonoBehaviour
{
    public GameObject minimap;

    private void Update()
    {
        minimap.SetActive(Input.GetKey(KeyCode.Tab) && (!EventSystem.current.currentSelectedGameObject ||
            !EventSystem.current.currentSelectedGameObject.GetComponent<InputField>()));
    }
}
