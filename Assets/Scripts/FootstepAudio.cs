using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class FootstepAudio : MonoBehaviour
{
    //public AudioMixer audioMixer;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    
    private AudioClip lastFootstep;
    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;
    private float lastPitch;
    private float pitchDir;

    private void PlayAudio()
    {
        if (audioClips.Length > 0)
        {
            lastFootstep = audioClips[0];
            //audioMixer.GetFloat("FootstepPitch", out lastPitch); //if audioMixer pitch is used instead
            lastPitch = audioSource.pitch;
            if (lastPitch == minPitch) pitchDir = 0.01f;
            else if (lastPitch == maxPitch) pitchDir = -0.01f;

            int next = Random.Range(1, 3);

            //audioMixer.SetFloat("FootstepPitch", Mathf.Clamp(UnityEngine.Random.Range(lastPitch - 0.053f + pitchDir, lastPitch + 0.03f) + pitchDir, minPitch, maxPitch)); //if audioMixer pitch is used instead
            audioSource.pitch = Mathf.Clamp(Random.Range(lastPitch - 0.03f + pitchDir, lastPitch + 0.03f + pitchDir), minPitch, maxPitch);
            audioSource.PlayOneShot(audioClips[next]);

            audioClips[0] = audioClips[next];
            audioClips[next] = lastFootstep;
        }
    }
}
