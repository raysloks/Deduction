using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowWallToggle : MonoBehaviour
{
    private ShadowCaster2D shadow;
    private Player player;

    private void Awake()
    {
        shadow = GetComponent<ShadowCaster2D>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        shadow.selfShadows = player.transform.position.y > transform.position.y;
    }
}
