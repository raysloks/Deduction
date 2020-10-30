using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Concentus.Structs;
using System.Text;
using System;
using System.Threading;

public class VoicePlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;

    private List<float> buffer = new List<float>();

    private OpusDecoder decoder;

    private int offset;

    private GameController game;

    private void Awake()
    {
        decoder = OpusDecoder.Create(48000, 1);

        audioSource = GetComponent<AudioSource>();
        audioClip = AudioClip.Create("Voice", 9600, 1, 48000, false);
        audioSource.clip = audioClip;

        game = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        audioSource.spatialBlend = game.phase == GamePhase.Setup || game.phase == GamePhase.Main ? 1f : 0f;
    }

    public void ProcessFrame(List<byte> frame)
    {
        int frameSize = 960;
        short[] outputBuffer = new short[frameSize];

        int thisFrameSize = decoder.Decode(frame.ToArray(), 0, frame.Count, outputBuffer, 0, frameSize, false);
        ProcessFrame(outputBuffer, thisFrameSize);
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public void ProcessFrame(short[] frame, int size)
    {
        float[] data = new float[size];
        for (int i = 0; i < size; ++i)
            data[i] = frame[i] / 32767f;

        if (mod(audioSource.timeSamples - offset + 4800, audioClip.samples) - 4800 > -1200)
            audioSource.timeSamples = mod(offset - 2400, audioClip.samples);

        audioClip.SetData(data, offset);
        offset += size;
        offset %= audioClip.samples;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
