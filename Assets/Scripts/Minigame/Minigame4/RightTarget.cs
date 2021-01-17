using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EventCallbacks;

public class RightTarget : MonoBehaviour
{
    public GameObject deathParticle;
    public GameObject textObject;

    public List<AudioClip> correctSounds;
    /*
    void OnMouseDown()
    {
        GameObject d = Instantiate(deathParticle, transform.position, Quaternion.identity);
        GameObject t = Instantiate(textObject, transform.position, Quaternion.identity);

        t.transform.SetParent(transform.parent);
        d.transform.SetParent(transform.parent);

        SoundEvent se = new SoundEvent();
        se.UnitSound = correctSounds;
        se.UnitGameObjectPos = transform.position;
        EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);

        DestroyImmediate(gameObject, true);

        // Destroy the gameObject after clicking on it
        Debug.Log("Right");
    }
    */

    public void Death()
    {
        GameObject d = Instantiate(deathParticle, transform.position, Quaternion.identity);
        GameObject t = Instantiate(textObject, transform.position, Quaternion.identity);

        t.transform.SetParent(transform.parent);
        d.transform.SetParent(transform.parent);

        SoundEvent se = new SoundEvent();
        se.UnitSound = correctSounds;
        se.UnitGameObjectPos = transform.position;
        EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);

        DestroyImmediate(gameObject, true);

        // Destroy the gameObject after clicking on it
        Debug.Log("Right");
    }
}
