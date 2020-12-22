using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public ushort index;
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    private void Awake()
    {
        DoorManager doorManager = FindObjectOfType<DoorManager>();
        if (doorManager.doors.ContainsKey(index))
            FindObjectOfType<GameController>().CreateInfoPopup("Duplicate door index " + index + ".");
        else
            doorManager.doors.Add(index, GetComponent<Animator>());

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int index)
    {
        if (index < audioClips.Length && audioClips[index] != null)
            audioSource.PlayOneShot(audioClips[index]);
    }
}
