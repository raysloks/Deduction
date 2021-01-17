using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public GameObject StartButton;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void PlaySound()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(clip);
        }

    }

}
