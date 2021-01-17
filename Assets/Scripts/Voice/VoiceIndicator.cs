using UnityEngine;
using System.Collections;
using System;

public class VoiceIndicator : MonoBehaviour
{
    public VoicePlayer voicePlayer;

    public Sprite[] sprites;
    public float[] limits;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (voicePlayer != null && voicePlayer.gameObject.activeInHierarchy)
        {
            float loudness = voicePlayer.GetLoudness();
            SetStage(loudness);
            spriteRenderer.enabled = loudness != float.NegativeInfinity;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
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
