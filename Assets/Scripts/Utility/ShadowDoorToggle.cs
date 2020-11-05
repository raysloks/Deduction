using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowDoorToggle : MonoBehaviour
{
    private Player player;

    public GameObject[] above;
    public GameObject[] below;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        bool isAbove = player.transform.position.y > transform.position.y;
        foreach (GameObject go in above)
            go.SetActive(isAbove);
        foreach (GameObject go in below)
            go.SetActive(!isAbove);
    }
}
