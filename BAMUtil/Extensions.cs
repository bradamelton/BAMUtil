using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BAMUtil
{
    public static class Extensions
    {
        public static T Pop<T>(this List<T> S)
        {
            var pop = S.FirstOrDefault();
            S.RemoveAt(0);
            return pop;
        }

        public static T Clone<T>(this T S)
        {
            T newObj = Activator.CreateInstance<T>();

            var sProps = S.GetType().GetProperties();

            foreach (PropertyInfo i in newObj.GetType().GetProperties())
            {
                //"EntitySet" is specific to link and this conditional logic is optional/can be ignored
                if (i.CanWrite && i.PropertyType.Name.Contains("EntitySet") == false)
                {
                    PropertyInfo sProp = sProps.First(p => p.Name == i.Name && p.GetType() == i.GetType());

                    i.SetValue(newObj, sProp.GetValue(S, null), null);
                }
            }

            return newObj;
        }

        public static List<List<T>> GetAllCombos<T>(this List<List<T>> S)
        {
            var results = new List<List<T>>();

            Depth<T>(S, 0, new List<T>(), results);

            return results;
        }

        private static void Depth<T>(List<List<T>> bigList, int currentDepth, List<T> currentStack, List<List<T>> results)
        {
            foreach (var l in bigList[currentDepth])
            {
                var nextStack = new List<T>();
                nextStack.AddRange(currentStack);
                nextStack.Add(l);

                if (currentDepth + 1 < bigList.Count)
                {
                    Depth(bigList, currentDepth + 1, nextStack, results);
                }
                else
                {
                    results.Add(nextStack);
                }
            }
        }
    }
}
