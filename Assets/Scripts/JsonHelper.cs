using System;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(FixJson(json));
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static string ToJson<T>(T[] array)
    {
        return ToJson(array, false);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    private static string FixJson(string value)
    {
        return "{\"Items\":" + value + "}";
    }
}