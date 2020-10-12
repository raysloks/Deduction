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

    }
    public class DebugEvent : Event
    {

    }

}
