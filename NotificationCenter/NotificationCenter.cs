using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter
{
    /// <summary>
    /// This class works like NSNotificationCenter in Objective-C or Swift.
    /// </summary>
    public static class NotificationCenter
    {
        #region Properties

        /// <summary>
        /// Keep action data if key was not subscribed yet. It just work in Notify with data.
        /// </summary>
        public static bool KeepActionValue { get; set; }

        private static Collection<Tuple<string, Action>> Subscribers { get; set; }

        private static Collection<Tuple<string, Action<object>>> SubscribersWithData { get; set; }

        private static List<Tuple<string, object>> KeepActionData { get; set; }

        #endregion
        /// <summary>
        /// Constructor for init subscriber collection.
        /// </summary>
        static NotificationCenter()
        {
            Subscribers = new Collection<Tuple<string, Action>>();
            SubscribersWithData = new Collection<Tuple<string, Action<object>>>();
            KeepActionData = new List<Tuple<string, object>>();
        }

        /// <summary>
        /// Subscribe an action for string key.
        /// </summary>
        /// <param name="key">String key for subscribe action name.</param>
        /// <param name="action">Action to call</param>
        public static void Subscribe(string key, Action action)
        {
            Subscribers.Add(new Tuple<string, Action>(key, action));
        }

        /// <summary>
        /// Subscribe an action with object for string key.
        /// </summary>
        /// <param name="key">String key for subscribe action name.</param>
        /// <param name="action">Action to call with object</param>
        public static void Subscribe(string key, Action<object> action)
        {
            SubscribersWithData.Add(new Tuple<string, Action<object>>(key, action));
        }

        /// <summary>
        /// Unsubscribe notification key in subscription list
        /// </summary>
        /// <param name="key">String key for unsubscribe action name.</param>
        public static void Unsubscribe(string key)
        {
            var listOfSubs = Subscribers.Where(k => k.Item1 == key).ToList();
            if (listOfSubs.Count > 0)
            {
                foreach (var sub in listOfSubs)
                {
                    Subscribers.Remove(sub);
                }
            }

            var listOfSubsWithOutActionObject = SubscribersWithData.Where(k => k.Item1 == key).ToList();
            if (listOfSubsWithOutActionObject.Count > 0)
            {
                foreach (var sub in listOfSubsWithOutActionObject)
                {
                    SubscribersWithData.Remove(sub);
                }
            }
        }

        /// <summary>
        /// Notify action for your key.
        /// </summary>
        /// <param name="key">String key for notify action</param>
        /// <returns>Task of notify</returns>
        public static Task Notify(string key)
        {
            return Task.Run(delegate
            {
                foreach (var subscription in Subscribers.Where(s => s.Item1 == key))
                {
                    Debug.WriteLine($"Your ('{key}') action was run completely ...");
                    subscription.Item2.Invoke();
                }
            });
        }

        /// <summary>
        /// Notify action for your key with a data.
        /// </summary>
        /// <param name="key">String key for notify action</param>
        /// <param name="data">Send data to subscribe on this key</param>
        /// <returns>Task of notify</returns>
        public static Task Notify(string key, object data)
        {
            return Task.Run(delegate
            {
                var subscription = SubscribersWithData.Where(s => s.Item1 == key).ToList();
                if (KeepActionValue)
                {
                    if (subscription.Count == 0)
                    {
                        KeepActionData.Add(new Tuple<string, object>(key, data));
                    }
                }

                var actions = KeepActionData.Where(k => k.Item1 == key).ToArray();

                foreach (var subscribe in subscription)
                {
                    if (KeepActionValue)
                    {
                        if (actions.Length > 0)
                        {
                            var datas = actions.Select(o => o.Item2).ToList();
                            if (datas.Count > 0)
                            {
                                Debug.WriteLine(
                                    $"Your ('{key}') action have value before you subscribed on ('{key}') key");
                                subscribe.Item2.Invoke(datas);
                            }
                        }
                    }

                    Debug.WriteLine($"Your ('{key}') action was run completely ...");
                    subscribe.Item2.Invoke(data);
                }
                if (KeepActionValue)
                {
                    foreach (var action in actions)
                    {
                        KeepActionData.Remove(action);
                    }
                }
            });
        }

    }
}
