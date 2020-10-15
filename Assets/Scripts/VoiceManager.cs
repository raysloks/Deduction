using UnityEngine;
using Concentus.Structs;
using Concentus.Enums;
using System.Text;
using System.Linq;
using System;

public class VoiceManager
{

    public NetworkHandler handler;

    private OpusEncoder encoder;

    private AudioClip recording;

    private int offset = 0;

    public VoiceManager()
    {
        encoder = OpusEncoder.Create(48000, 1, OpusApplication.OPUS_APPLICATION_AUDIO);
        encoder.Bitrate = 12000;

        recording = Microphone.Start("", true, 1, 48000);
        float[] data = new float[recording.samples];
        for (int i = 0; i < data.Length; ++i)
            data[i] = (((i + offset) % 2) == 0) ? 0.9f : -0.9f;
        recording.SetData(data, 0);
    }

    public void Stream()
    {
        {
            int frameSize = 960;
            short[] inputAudioSamples = new short[frameSize];
            if (Poll(inputAudioSamples))
            {
                //handler.controller.voicePlayer.ProcessFrame(inputAudioSamples);
                byte[] outputBuffer = new byte[1000];

                if (!Input.GetKey(KeyCode.V))
                    for (int i = 0; i < inputAudioSamples.Length; ++i)
                        inputAudioSamples[i] = 0;

                for (int i = 0; i < inputAudioSamples.Length - 1; ++i)
                {
                    Debug.DrawLine(new Vector3(i / 100f, inputAudioSamples[i] / 10000f), new Vector3((i + 1) / 100f, inputAudioSamples[i + 1] / 10000f), Color.white);
                }

                int thisPacketSize = encoder.Encode(inputAudioSamples, 0, frameSize, outputBuffer, 0, outputBuffer.Length);

                //byte[] frame = new byte[thisPacketSize];
                //handler.controller.voicePlayer.ProcessFrame(outputBuffer);

                VoiceFrame message = new VoiceFrame();
                message.data = outputBuffer.Take(thisPacketSize).ToList();
                handler.link.Send(message);
                //handler.controller.voicePlayer.ProcessFrame(message.data);
            }
            else
            {
            }
        }
    }

    private bool Poll(short[] samples)
    {
        float[] data = new float[samples.Length + 16];
        if (!recording.GetData(data, offset))
            return false;
        int matches = 0;
        float[] replacement = new float[samples.Length];
        for (int i = 0; i < data.Length; ++i)
        {
            float pattern = (((i + offset) % 2) == 0) ? 0.9f : -0.9f;
            if (Mathf.Abs(data[i] - pattern) < 0.1f)
                ++matches;
            else
                matches = 0;
            if (i < samples.Length)
            {
                samples[i] = (short)(data[i] * 32767f);
                replacement[i] = pattern;
            }
        }
        if (matches > 8)
            return false;
        recording.SetData(replacement, offset);
        offset += samples.Length;
        offset %= recording.samples;
        return true;
    }
}
