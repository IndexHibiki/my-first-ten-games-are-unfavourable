using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtil
{
    public static string toJson<T>(T obj)
    {
        if (obj == null) return "null";

        if (typeof(T).GetInterface("IList") != null)
        {
            Pack<T> pack = new Pack<T>();
            pack.data = obj;
            string json = JsonUtility.ToJson(pack);

            return json.Substring(8, json.Length - 9);
        }

        return JsonUtility.ToJson(obj);
    }

    public static T fromJson<T>(string json)
    {
        if (json == "null" && typeof(T).IsClass) return default(T);

        if (typeof(T).GetInterface("IList") != null)
        {
            json = "{\"data\":{data}}".Replace("{data}", json);
            Pack<T> Pack = JsonUtility.FromJson<Pack<T>>(json);

            return Pack.data;
        }

        return JsonUtility.FromJson<T>(json);
    }

    private class Pack<T>
    {
        public T data;
    }

}
