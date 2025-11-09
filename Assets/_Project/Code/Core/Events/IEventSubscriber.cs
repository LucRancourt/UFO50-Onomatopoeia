using System;
using System.Collections.Generic;


namespace _Project.Code.Core.Events
{
    public interface IEventSubscriber
    {
        public void ClearEventSubscriberOnDisables();
    }


    public static class EventSubscriberMethods
    {
        private static readonly Dictionary<IEventSubscriber, List<Action>> _unsubscribeActions = new();

        public static void Subscribe<T>(this IEventSubscriber subscriber, Action<T> callback) where T : IEvent
        {
            if (EventBus.Instance)
            {
                EventBus.Instance.Subscribe(subscriber, callback);

                if (!_unsubscribeActions.ContainsKey(subscriber))
                    _unsubscribeActions[subscriber] = new List<Action>();

                _unsubscribeActions[subscriber].Add(() => EventBus.Instance?.Unsubscribe<T>(subscriber));
            }
        }

        public static void OnDestroyEventSubscriber(this IEventSubscriber subscriber)
        {
            if (_unsubscribeActions.TryGetValue(subscriber, out var actionList))
            {
                foreach (var action in actionList)
                    action();

                actionList.Clear();
            }
        }
    }
}



