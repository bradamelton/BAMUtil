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
    }
}
