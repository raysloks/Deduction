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
        public GameController game;
        public ulong idOfInitiator;
        public ulong idOfBody;
    }

    public class VoteEvent : Event
    {
        public ulong idOfTarget;
        public ulong idOfVoter;
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

    public class SettingEvent : Event
    {
        public GameSettings settings;
    }

    public class PhaseChangedEvent : Event
    {
        public GamePhase phase;
        public long timer;
        public GamePhase previous;
    }

}
