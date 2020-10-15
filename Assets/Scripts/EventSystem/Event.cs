using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public abstract class Event
    {
        public string EventDescription;
    }

    public class MeetingEvent : Event
    {
        public NetworkHandler meetingHandler;

    }
    public class VoteEvent : Event
    {
        public string nameOfButton;
        public int totalAmountOfVotes;
        public ulong idOfVoter;
        public bool doneVoting;

    }

    public class MeetingDieEvent : Event
    {
        public Vector3 UnitGameObjectPos;
        public GameObject UnitGameObject;
        public AudioClip UnitSound;
        public GameObject UnitParticle;
    }
    public class DebugEvent : Event
    {

    }

}
