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

    private void Awake()
    {
        decoder = OpusDecoder.Create(48000, 1);

        audioSource = GetComponent<AudioSource>();
        audioClip = AudioClip.Create("Voice", 4800, 1, 48000, false);
        audioSource.clip = audioClip;
    }

    private void ReadData(float[] data)
    {
        int length = Math.Min(data.Length, buffer.Count);
        for (int i = 0; i < length; ++i)
            data[i] = buffer[i];
        for (int i = length; i < data.Length; ++i)
            data[i] = 0f;
        buffer.RemoveRange(0, length);
    }

    public void ProcessFrame(List<byte> frame)
    {
        int frameSize = 960;
        short[] outputBuffer = new short[frameSize];

        int thisFrameSize = decoder.Decode(frame.ToArray(), 0, frame.Count, outputBuffer, 0, frameSize, false);
        ProcessFrame(outputBuffer, thisFrameSize);
    }

    public void ProcessFrame(short[] frame, int size)
    {
        float[] data = new float[size];
        for (int i = 0; i < size; ++i)
            data[i] = frame[i] / 32767f;

        audioClip.SetData(data, offset);
        offset += size;
        offset %= audioClip.samples;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
