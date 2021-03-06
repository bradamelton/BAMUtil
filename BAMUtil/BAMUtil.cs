﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Net;
using System.ComponentModel;

namespace BAMUtil
{
    public static class Util
    {
        // not sure if this will work on inherited handlers. Example from: https://stackoverflow.com/questions/91778/how-to-remove-all-event-handlers-from-an-event
        // maybe BindingFlags.FlattenHierarchy | BindingFlags.Instance
        public static void RemoveHandlers(object o1, string eventName)
        {
            var o1Type = o1.GetType();

            EventInfo f1 = o1Type.GetEvent(eventName);

            var field = o1Type.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            var eventDelegate = field?.GetValue(o1);

            if (eventDelegate != null)
            {
                f1?.RemoveEventHandler(o1, (Delegate)eventDelegate);
            }
        }
        public static string ObjectToQueryString(object o1, bool urlEncode = true)
        {
            var sb = new StringBuilder();

            if (o1 != null)
            {
                Type ot1 = o1.GetType();

                PropertyInfo[] o1pi = ot1.GetProperties();

                foreach (PropertyInfo pi1 in o1pi)
                {
                    object val = pi1.GetValue(o1);

                    if (val != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("&");
                        }

                        sb.Append($"{pi1.Name}={WebUtility.UrlEncode(val.ToString())}");
                    }
                }
            }

            return sb.ToString();
        }

        public static string ObjectToString(object o1, StringBuilder sb = null, int level = 0)
        {
            if (o1 != null)
            {
                Type ot1 = o1.GetType();

                if (sb == null)
                {
                    sb = new StringBuilder();
                    sb.AppendLine($"{ot1.Name}: ");
                }

                if (isEnumerable(ot1))
                {
                    if (level > 0)
                    {
                        sb.AppendLine($"{string.Empty.PadLeft((level) * 3)}{ot1.Name}: ");
                    }

                    IEnumerable<object> o1E = (IEnumerable<object>)o1;

                    for (int i = 0; i < ((IEnumerable<object>)o1).Count(); i++)
                    {
                        if (o1E.ElementAt(i) != null)
                        {
                            ObjectToString(o1E.ElementAt(i), sb, level + 1); // same level, because we already went down.
                        }
                    }
                }
                else
                {
                    // I would like a more general way of doing this.
                    if (ot1 == typeof(string) || ot1 == typeof(System.Object) || ot1.BaseType == typeof(System.ValueType) || ot1.Namespace.StartsWith("System"))
                    {
                        sb.AppendLine($"{string.Empty.PadLeft((level) * 3)}{o1.ToString()}");
                    }
                    else
                    {
                        PropertyInfo[] o1pi = ot1.GetProperties();

                        foreach (PropertyInfo pi1 in o1pi)
                        {
                            object val = null;

                            try
                            {
                                sb.Append($"{string.Empty.PadLeft((level + 1) * 3)}{pi1.Name}: ");
                                val = pi1.GetValue(o1);

                                if (val == null)
                                {
                                    sb.AppendLine();
                                }
                                else
                                {
                                    ObjectToString(val, sb, level + 1);
                                }
                            }
                            catch (Exception ex)
                            {
                                sb.AppendLine($"{ex.Message}.");
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static bool isEnumerable(Type ot1)
        {
            // string is technically an array of chars.
            if (ot1 == typeof(string)) { return false; }

            var found = (from i in ot1.GetInterfaces()
                         where i.IsGenericType &&
i.GetGenericTypeDefinition() == typeof(IEnumerable<>) //&&
                                                      // typeof(MyBaseClass).IsAssignableFrom(i.GetGenericArguments()[0])
                         select i);

            return found?.Count() > 0;
        }
    }

    public class InputListener
    {
        public static CancellationTokenSource StartListener(string message = "", Action callback = null)
        {
            var cts = new CancellationTokenSource();

            var token = cts.Token;

            var task = Task.Run(() =>
            {
                Console.WriteLine(message);
                Console.ReadLine();
                cts?.Cancel();
                callback?.Invoke();
            });

            return cts;
        }
    }
}