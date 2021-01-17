using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class VoiceIndicatorImage : MonoBehaviour
{
    public VoicePlayer voicePlayer;

    public Sprite[] sprites;
    public float[] limits;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (voicePlayer != null && voicePlayer.gameObject.activeInHierarchy)
        {
            float loudness = voicePlayer.GetLoudness();
            SetStage(loudness);
            image.enabled = loudness != float.NegativeInfinity;
        }
        else
        {
            image.enabled = false;
        }
    }

    public void SetStage(float loudness)
    {
        for (int i = 0; i < limits.Length; ++i)
        {
            if (limits[i] > loudness)
            {
                if (i < sprites.Length)
                    image.sprite = sprites[i];
                return;
            }
        }
        if (sprites.Length > 0)
            image.sprite = sprites[sprites.Length - 1];
    }
}
