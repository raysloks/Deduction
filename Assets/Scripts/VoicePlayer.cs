using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Concentus.Structs;
using System.Text;
using System;
using System.Threading;
using System.Linq;

public class VoicePlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;

    private OpusDecoder decoder;

    private int offset;

    private GameController game;

    private List<long> sums = new List<long>();
    private List<long> square_sums = new List<long>();

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

        if (mod(audioSource.timeSamples - offset, audioClip.samples) > audioClip.samples - 960)
        {
            audioSource.Stop();
            sums.Clear();
        }
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

    public float GetLoudness()
    {
        if (sums.Count > 0)
        {
            long sum = 0;
            for (int i = 0; i < sums.Count; ++i)
                sum += sums[i];
            return 10f * Mathf.Log10((sum / (960 * sums.Count)) / 32767f);
        }
        return float.NegativeInfinity;
    }

    public void ProcessFrame(short[] frame, int size)
    {
        float[] data = new float[size];
        long sum = 0;
        long square_sum = 0;
        for (int i = 0; i < size; ++i)
        {
            data[i] = frame[i] / 32767f;
            square_sum += frame[i] * frame[i];
            sum += Math.Abs((long)frame[i]);
        }
        sums.Add(sum);
        sums.RemoveRange(0, Math.Max(0, sums.Count - 5));
        square_sums.Add(square_sum);
        square_sums.RemoveRange(0, Math.Max(0, sums.Count - 5));

        if (mod(audioSource.timeSamples - offset + 4800, audioClip.samples) - 4800 > -1200)
            audioSource.timeSamples = mod(offset - 2400, audioClip.samples);

        audioClip.SetData(data, offset);
        offset += size;
        offset %= audioClip.samples;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
