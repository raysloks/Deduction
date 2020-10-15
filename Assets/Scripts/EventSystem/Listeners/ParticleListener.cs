using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class ParticleListener : MonoBehaviour
    {
        private Vector3 diePlace;
        // Start is called before the first frame update
        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.MEETING_DIED, OnDiePlayParticleEffect);

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
    }
}