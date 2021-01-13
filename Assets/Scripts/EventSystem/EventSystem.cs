using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class EventSystem : MonoBehaviour
    {
        static private EventSystem __Current;
        static public EventSystem Current
        {
            get
            {
                if (__Current == null)
                {
                    __Current = FindObjectOfType<EventSystem>();
                }

                return __Current;
            }
        }

        public delegate void EventListener(Event ei);
        Dictionary<EVENT_TYPE, List<EventListener>> eventListeners;

        public void RegisterListener(EVENT_TYPE eventType, EventListener listener)
        {
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<EVENT_TYPE, List<EventListener>>();
            }

            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new List<EventListener>();
            }

            eventListeners[eventType].Add(listener);
        }

        public void UnregisterListener(EVENT_TYPE eventType, EventListener listener)
        {
            if (eventListeners.ContainsKey(eventType) == true && eventListeners[eventType].Contains(listener))
            {
                eventListeners[eventType].Remove(listener);
            }
        }

        public void FireEvent(EVENT_TYPE eventType, Event eventInfo)
        {
            if (eventListeners == null || eventListeners[eventType] == null)
            {
                return;
            }

            foreach (EventListener el in eventListeners[eventType])
            {
                try
                {
                    el(eventInfo);
                }
                catch
                {

                }
            }
        }
    }
}
