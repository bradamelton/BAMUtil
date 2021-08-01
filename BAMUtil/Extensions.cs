using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BAMUtil {
    public static class Extensions {
        // dynamic here is a little trixie. I really need a nullable t.
        public static dynamic Pop<T>(this List<T> S) where T : new() {
            if (S.Count > 0) {
                var pop = S.FirstOrDefault();
                S.RemoveAt(0);
                return pop;
            }

            return null;
        }

        public static void CloneInto<T>(this T obj, object objOut) {
            // if objOut is null, should we instanciate? Prolly not.
            if (obj != null) {
                var sProps = obj?.GetType().GetProperties();

                foreach (PropertyInfo i in objOut?.GetType().GetProperties()) {
                    //"EntitySet" is specific to link and this conditional logic is optional/can be ignored
                    if (i.CanWrite && i.PropertyType.Name.Contains("EntitySet") == false) {
                        PropertyInfo sProp = sProps?.FirstOrDefault(p => p.Name == i.Name && p.GetType() == i.GetType());

                        if (sProp != null) {
                            i.SetValue(objOut, sProp.GetValue(obj, null), null);
                        }
                    }
                }
            }
        }

        public static T Clone<T>(this T S) {
            T newObj = Activator.CreateInstance<T>();

            var sProps = S.GetType().GetProperties();

            foreach (PropertyInfo i in newObj.GetType().GetProperties()) {
                //"EntitySet" is specific to link and this conditional logic is optional/can be ignored
                if (i.CanWrite && i.PropertyType.Name.Contains("EntitySet") == false) {
                    PropertyInfo sProp = sProps.First(p => p.Name == i.Name && p.GetType() == i.GetType());

                    i.SetValue(newObj, sProp.GetValue(S, null), null);
                }
            }

            return newObj;
        }

        public static List<List<T>> GetAllCombos<T>(this List<List<T>> S) {
            var results = new List<List<T>>();

            Depth<T>(S, 0, new List<T>(), results);

            return results;
        }

        private static void Depth<T>(List<List<T>> bigList, int currentDepth, List<T> currentStack, List<List<T>> results) {
            foreach (var l in bigList[currentDepth]) {
                var nextStack = new List<T>();
                nextStack.AddRange(currentStack);
                nextStack.Add(l);

                if (currentDepth + 1 < bigList.Count) {
                    Depth(bigList, currentDepth + 1, nextStack, results);
                }
                else {
                    results.Add(nextStack);
                }
            }
        }
    }
}
