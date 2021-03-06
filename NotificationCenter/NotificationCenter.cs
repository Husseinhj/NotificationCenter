﻿using System;
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

        /// <summary>
        /// Show logs if it was true
        /// </summary>
        private static bool EnableLogs = false;

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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "KEY argument should have value");
            }

            Subscribers?.Add(new Tuple<string, Action>(key, action));
        }

        /// <summary>
        /// Subscribe an action with object for string key.
        /// </summary>
        /// <param name="key">String key for subscribe action name.</param>
        /// <param name="action">Action to call with object</param>
        public static void Subscribe(string key, Action<object> action)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "KEY argument should have value");
            }

            SubscribersWithData?.Add(new Tuple<string, Action<object>>(key, action));
        }

        /// <summary>
        /// Unsubscribe notification key in subscription list
        /// </summary>
        /// <param name="key">String key for unsubscribe action name.</param>
        public static void Unsubscribe(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key),"KEY argument should have value");
            }

            var listOfSubs = Subscribers?.Where(k => k.Item1 == key).FirstOrDefault();
            if (listOfSubs != null)
            {
                Subscribers?.Remove(listOfSubs);
            }

            var listOfSubsWithOutActionObject = SubscribersWithData?.Where(k => k.Item1 == key).SingleOrDefault();
            if (listOfSubsWithOutActionObject != null)
            {
                SubscribersWithData?.Remove(listOfSubsWithOutActionObject);
            }
        }

        /// <summary>
        /// Unsubscribe to all notification keys.
        /// </summary>
        public static void UnsubscribeAll()
        {
            Subscribers?.Clear();
            SubscribersWithData?.Clear();
        }

        /// <summary>
        /// Notify action for your key.
        /// </summary>
        /// <param name="key">String key for notify action</param>
        /// <returns>Task of notify</returns>
        public static Task Notify(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "KEY argument should have value");
            }

            return Task.Run(delegate
            {
                var subscriptions = Subscribers.Where(s => s.Item1 == key);

                subscriptions.AsParallel().ForAll((actions) =>
                {
                    if (EnableLogs)
                    {
                        Debug.WriteLine($"Your ('{key}') action was run completely ...");
                    }
                    actions.Item2.Invoke();
                });
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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "KEY argument should have value");
            }

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
                if (subscription.Count > 0)
                {
                    subscription.AsParallel().ForAll((Tuple<string, Action<object>> action) =>
                     {
                         if (actions.Length > 0)
                         {
                             var datas = actions.Select(o => o.Item2).ToList();
                             if (datas.Count > 0)
                             {
                                 if (EnableLogs)
                                 {
                                     Debug.WriteLine(
                                     $"Your ('{key}') action have value before you subscribed on ('{key}') key");
                                 }
                                 action.Item2.Invoke(datas);
                             }
                         }
                         if (EnableLogs)
                         {
                             Debug.WriteLine($"Your ('{key}') action was run completely ...");
                         }
                         action.Item2.Invoke(data);
                     });
                }
                KeepActionData.Remove(actions.FirstOrDefault());
            });
        }

    }
}
