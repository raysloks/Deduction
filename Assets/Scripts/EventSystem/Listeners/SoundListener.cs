using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class SoundListener : MonoBehaviour
    {
        private int NoOfcurrentlyPlayingSounds = 0;
        [SerializeField] private int maxNoOfSimultaneousSounds;

        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_DIED, OnDiePlaySound);
            EventSystem.Current.RegisterListener(EVENT_TYPE.PLAY_SOUND, GeneralPlaySound);
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

        void GeneralPlaySound(Event eventInfo)
        {
            SoundEvent soundEventInfo = (SoundEvent)eventInfo;
            List<AudioClip> sound = soundEventInfo.UnitSound;

            if (NoOfcurrentlyPlayingSounds < maxNoOfSimultaneousSounds && sound != null)
            {
                foreach(AudioClip s in sound)
                {
                    if (NoOfcurrentlyPlayingSounds > maxNoOfSimultaneousSounds)
                    {
                        break;
                    }
                    NoOfcurrentlyPlayingSounds++;
                    AudioSource.PlayClipAtPoint(s, soundEventInfo.UnitGameObjectPos);
                    Invoke("SubtractCurrentlyPlayingSounds", s.length);
                }               

            }
        }

        public void SubtractCurrentlyPlayingSounds()
        {
            NoOfcurrentlyPlayingSounds--;
        }
    }
}
