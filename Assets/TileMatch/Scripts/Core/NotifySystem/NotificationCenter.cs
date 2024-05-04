using System;
using System.Collections.Generic;

namespace TowerDefence.Core.NotifySystem
{
    public static class NotificationCenter
    {
       private static readonly Dictionary<string, Delegate> EventDictionary = new();

        public static void AddObserver(string eventName, Action observer)
        {
            AddObserverInternal(eventName, observer);
        }

        public static void AddObserver<T>(string eventName, Action<T> observer)
        {
            AddObserverInternal(eventName, observer);
        }

        public static void AddObserver<T1, T2>(string eventName, Action<T1, T2> observer)
        {
            AddObserverInternal(eventName, observer);
        }

        public static void RemoveObserver(string eventName, Action observer)
        {
            RemoveObserverInternal(eventName, observer);
        }

        public static void RemoveObserver<T>(string eventName, Action<T> observer)
        {
            RemoveObserverInternal(eventName, observer);
        }

        public static void RemoveObserver<T1, T2>(string eventName, Action<T1, T2> observer)
        {
            RemoveObserverInternal(eventName, observer);
        }

        public static void PostNotification(string eventName)
        {
            PostNotificationInternal(eventName);
        }

        public static void PostNotification<T>(string eventName, T arg)
        {
            PostNotificationInternal(eventName, arg);
        }

        public static void PostNotification<T1, T2>(string eventName, T1 arg1, T2 arg2)
        {
            PostNotificationInternal(eventName, arg1, arg2);
        }

        private static void AddObserverInternal(string eventName, Delegate observer)
        {
            EventDictionary.TryAdd(eventName, null);
            EventDictionary[eventName] = Delegate.Combine(EventDictionary[eventName], observer);
        }

        private static void RemoveObserverInternal(string eventName, Delegate observer)
        {
            if (EventDictionary.ContainsKey(eventName))
            {
                EventDictionary[eventName] = Delegate.Remove(EventDictionary[eventName], observer);

                if (EventDictionary[eventName] == null)
                {
                    EventDictionary.Remove(eventName);
                }
            }
        }

        private static void PostNotificationInternal(string eventName, params object[] args)
        {
            if (EventDictionary.TryGetValue(eventName, out var value))
            {
                value?.DynamicInvoke(args);
            }
        }
    }
}