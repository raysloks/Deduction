﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class SoundListener : MonoBehaviour
    {
        private int NoOfcurrentlyPlayingSounds = 0;
        [SerializeField] private int maxNoOfSimultaneousSounds;

        // Start is called before the first frame update
        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_DIED, OnDiePlaySound);
        }

        void OnDiePlaySound(Event eventInfo)
        {
            MeetingDieEvent dieEventInfo = (MeetingDieEvent)eventInfo;
            AudioClip dieSound = dieEventInfo.UnitSound;

            if (NoOfcurrentlyPlayingSounds < maxNoOfSimultaneousSounds && dieSound != null)
            {
                NoOfcurrentlyPlayingSounds++;
                AudioSource.PlayClipAtPoint(dieSound, dieEventInfo.UnitGameObjectPos);
                Invoke("SubtractCurrentlyPlayingSounds", dieSound.length);


            }
        }

        public void SubtractCurrentlyPlayingSounds()
        {
            NoOfcurrentlyPlayingSounds--;
        }
    }
}
