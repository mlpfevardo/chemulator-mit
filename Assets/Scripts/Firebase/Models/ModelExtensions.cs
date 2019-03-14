using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ModelExtensions
{
    //public static T ToObject<T>(this IDictionary<string, object> source)
    //    where T : class, new()
    //{
    //    return GetObject(typeof(T), source) as T;
    //}

    //private static object GetObject(Type t, IDictionary<string, object> source)
    //{
    //    var someObject = Activator.CreateInstance(t);
    //    var someObjectType = t;
    //    PropertyInfo property;

    //    foreach (var item in source)
    //    {
    //        property = someObjectType
    //                     .GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

    //        try
    //        {
    //            if (item.Value.GetType() == typeof(Int64))
    //            {
    //                property.SetValue(someObject, (int)item.Value, null);
    //            }
    //            else
    //            {
    //                property.SetValue(someObject, item.Value, null);
    //            }
    //        }
    //        catch (NullReferenceException)
    //        {
    //            Debug.LogWarning("ToObject failed: Cannot find property " + item.Key + " of " + someObjectType.ToString());
    //        }
    //        catch (ArgumentException)
    //        {
    //            if (item.Value.GetType() == typeof(Dictionary<string, object>))
    //            {
    //                var newValue = GetObject(property.PropertyType, item.Value as Dictionary<string, object>);

    //                someObjectType
    //                     .GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
    //                     .SetValue(someObject, newValue, null);

    //            }
    //            else
    //            {
    //                Debug.LogWarning("ToObject failed: Unsupported type for value " + item.Value.GetType());
    //            }
    //        }
    //    }

    //    return someObject;
    //}
}