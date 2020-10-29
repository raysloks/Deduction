﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EventCallbacks;

public class EjectionScreen : MonoBehaviour
{
    public GameObject ejectedPrefab;

    private void Awake()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.MOB_EJECTED, x => Ejected((MobEjectedEvent)x));
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, x => PhaseChanged((PhaseChangedEvent)x));

        gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (Transform child in transform)
        {
            child.Rotate(Vector3.back, Time.deltaTime * 60.0f);
            child.localScale *= Mathf.Exp(Time.deltaTime * Mathf.Log(0.5f));
        }
    }

    private void Ejected(MobEjectedEvent mobEjectedEvent)
    {
        Mob mob = mobEjectedEvent.mob;
        var go = Instantiate(ejectedPrefab, transform);
        var image = go.GetComponent<Image>();
        image.sprite = mob.sprites[0];
        image.color = mob.sprite.color;
        var text = go.GetComponentInChildren<Text>();
        text.text = mob.name;
    }

    private void PhaseChanged(PhaseChangedEvent phaseChangedEvent)
    {
        if (phaseChangedEvent.phase == GamePhase.Ejection)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}
