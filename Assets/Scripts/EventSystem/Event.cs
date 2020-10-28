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

    public class SoundEvent : Event
    {
        public List<AudioClip> UnitSound;
        public Vector3 UnitGameObjectPos;
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

    public class GameOverEvent : Event
    {
        public List<Mob> winners;
        public bool victory;
        public ulong role;
    }

}
