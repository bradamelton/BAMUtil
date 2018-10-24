using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;

namespace BAMUtil
{
    public static class Util
    {
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
}
