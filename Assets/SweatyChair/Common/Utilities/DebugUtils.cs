using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SweatyChair;

public static class DebugUtils
{

    public static bool CheckPlaying()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Please run while game is playing.");
            return false;
        }
        return true;
    }

    public static bool CheckNotPlaying()
    {
        if (Application.isPlaying)
        {
            Debug.LogError("Please run while game is NOT playing.");
            return false;
        }
        return true;
    }

    public static void ReloadScene()
    {
        if (Application.isPlaying)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void Log(object[] array, string arrayName = "", string seperator = ",")
    {
        Debug.Log((string.IsNullOrEmpty(arrayName) ? "" : (arrayName + "=")) + StringUtils.ArrayToString(array, seperator));
    }

    public static void Log(IList list, string listName = "", string seperator = ",")
    {
        Debug.Log((string.IsNullOrEmpty(listName) ? "" : (listName + "=")) + StringUtils.ListToString(list, seperator));
    }

    public static void Log(IDictionary dict, string dictName = "")
    {
        Debug.Log((string.IsNullOrEmpty(dictName) ? "" : (dictName + "=")) + StringUtils.DictionaryToString(dict));
    }

    public static void Log(object obj, string objName = "")
    {
        if (obj == null)
            Debug.Log((string.IsNullOrEmpty(objName) ? "" : (objName + "=")) + "null");
        else if (obj is IList)
            Log((IList)obj, objName);
        else if (obj is IDictionary)
            Log((IDictionary)obj, objName);
        else
            Debug.Log((string.IsNullOrEmpty(objName) ? "" : (objName + "=")) + obj);
    }

    public static void LogCollection(ICollection coll, string collName = "", string seperator = ",")
    {
        Debug.Log((string.IsNullOrEmpty(collName) ? "" : (collName + ": ")) + StringUtils.CollectionToString(coll, seperator));
    }

    // Log each element in a row
    public static void LogEach(object[] objs, string arrayName = "")
    {
        if (objs == null)
        {
            Debug.Log((string.IsNullOrEmpty(arrayName) ? "" : (arrayName + "=")) + "null");
            return;
        }
        List<object> objList = new List<object>(objs);
        objList.RemoveAll(o => o == null);
        Debug.LogFormat("There's {0} in {1}:", objList.Count, arrayName);
        foreach (object o in objList)
            Debug.Log(o);
    }

    public static void LogEach(IList list, string listName = "")
    {
        Debug.LogFormat("There's {0} in {1}:", list.Count, listName);
        foreach (object o in list)
            Debug.Log(o);
    }

    public static void LogEach(IDictionary dict, string dictName = "")
    {
        Debug.LogFormat("There's {0} in {1}:", dict.Count, dictName);
        foreach (var o in dict.Values)
            Debug.Log(o);
    }

    public static void LogEach(ICollection coll, string collName = "")
    {
        Debug.LogFormat("There's {0} in {1}:", coll.Count, collName);
        foreach (object o in coll)
            Debug.Log(o);
    }

    public static void LogPlayerPrefs<T>(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            Log(null, key);
        else if (typeof(T) == typeof(int))
            Log(PlayerPrefs.GetInt(key), key);
        else if (typeof(T) == typeof(string))
            Log(PlayerPrefs.GetString(key), key);
        else if (typeof(T) == typeof(float))
            Log(PlayerPrefs.GetFloat(key), key);
        else if (typeof(T) == typeof(DateTime))
            Log(DateTimeUtils.GetPlayerPrefs(key), key);
        else if (typeof(T) == typeof(int[]))
            Log(PlayerPrefsX.GetIntArray(key), key);
        else if (typeof(T) == typeof(float[]))
            Log(PlayerPrefsX.GetFloatArray(key), key);
        else if (typeof(T) == typeof(bool[]))
            Log(PlayerPrefsX.GetBoolArray(key), key);
        else if (typeof(T) == typeof(string[]))
            Log(PlayerPrefsX.GetStringArray(key), key);
        else if (typeof(T) == typeof(Vector3))
            Log(PlayerPrefsX.GetVector3(key), key);
        else if (typeof(T) == typeof(DateTime[]))
            Log(DateTimeUtils.GetPlayerPrefsX(key), key);
        else if (typeof(T) == typeof(List<int>))
            Log(PlayerPrefsX.GetIntList(key), key);
        else if (typeof(T) == typeof(List<bool>))
            Log(PlayerPrefsX.GetBoolList(key), key);
        else if (typeof(T) == typeof(List<string>))
            Log(PlayerPrefsX.GetStringList(key), key);
    }

    //public static void LogGameSave<T>(string key)
    //{
    //    Log(GameSave.Get<T>(key), key);
    //}

    #region CUDLR Utilities

    public static bool CUDLRGetInt(string command, string[] args, out int result)
    {
        result = 0;

        if (args.Length != 1)
        {
            Debug.LogErrorFormat("[{0}] Expected '{0}' <int>", command);
            return false;
        }

        if (DebugUtils.CheckPlaying())
        {
            if (int.TryParse(args[0], out result))
            {
                return true;
            }
            else
            {
                Debug.LogErrorFormat("[{0}] could not parse '{1}' to int", command, args[0]);
            }
        }
        return false;
    }

    #endregion
}