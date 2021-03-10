using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public static class ParamExtention
{
    public static void AddorNull<T>(this Param param, string key, T value) where T : class
    {
        if (value == null)
        {
            param.AddNull(key);
        }
        else
        {
            param.Add(key, value);
        }
    }

    public static void AddorNull<T>(this Param param, string key, List<T> value) where T : class
    {
        if (value == null)
        {
            param.AddNull(key);
        }
        else if (value.Count == 0)
        {
            param.AddNull(key);
        }
        else
        {
            param.Add(key, value);
        }
    }
}
