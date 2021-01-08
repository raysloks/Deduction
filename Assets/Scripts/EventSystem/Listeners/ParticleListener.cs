using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class ParticleListener : MonoBehaviour
    {
        private Vector3 diePlace;

        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_DIED, OnDiePlayParticleEffect);
            EventSystem.Current.RegisterListener(EVENT_TYPE.KNIFE_KILL, OnDiePlayParticleEffect2);
        }

        void OnDiePlayParticleEffect(Event eventInfo)
        {
            MeetingDieEvent unitDieEvent = (MeetingDieEvent)eventInfo;
            diePlace = unitDieEvent.UnitGameObjectPos;
            GameObject myParticle = unitDieEvent.UnitParticle;
            
            if (myParticle != null && diePlace != null)
            {
                GameObject particle = Instantiate(myParticle, diePlace, Quaternion.identity);
            }
        }

        void OnDiePlayParticleEffect2(Event eventInfo)
        {
            KnifeDieEvent unitDieEvent = (KnifeDieEvent)eventInfo;
            diePlace = unitDieEvent.UnitGameObjectPos;
            GameObject myParticle = unitDieEvent.UnitParticle;

            if (myParticle != null && diePlace != null)
            {
                GameObject particle = Instantiate(myParticle, diePlace, Quaternion.identity);
            }
        }
    }
}