using UnityEngine;
using System.Collections;
using System;

public class VoiceIndicator : MonoBehaviour
{
    public Sprite[] sprites;
    public float[] limits;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetStage(float loudness)
    {
        for (int i = 0; i < limits.Length; ++i)
        {
            if (limits[i] > loudness)
            {
                if (i < sprites.Length)
                    spriteRenderer.sprite = sprites[i];
                return;
            }
        }
        if (sprites.Length > 0)
            spriteRenderer.sprite = sprites[sprites.Length - 1];
    }
}
