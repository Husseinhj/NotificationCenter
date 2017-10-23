using System;
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
        private static Collection<Tuple<string, Action>> Subscribers { get; set; }
        private static Collection<Tuple<string, Action<object>>> SubscribersWithData { get; set; }

        /// <summary>
        /// Constructor for init subscriber collection.
        /// </summary>
        static NotificationCenter()
        {
            Subscribers = new Collection<Tuple<string, Action>>();
            SubscribersWithData = new Collection<Tuple<string, Action<object>>>();
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
                foreach (var subscription in SubscribersWithData.Where(s => s.Item1 == key))
                {
                    Debug.WriteLine($"Your ('{key}') action was run completely ...");
                    subscription.Item2.Invoke(data);
                }
            });
        }

    }
}
