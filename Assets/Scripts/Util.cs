using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static Vector3 WithX(Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static T InstantiateAs<T>(Component prefab, Vector2 pos, Transform t)
    {
        var instance = GameObject.Instantiate(prefab, pos, Quaternion.identity, t);
        return instance.GetComponent<T>();
    }

    public static T InstantiateAs<T>(Component prefab, Vector2 pos, Quaternion rot, Transform t)
    {
        var instance = GameObject.Instantiate(prefab, pos, rot, t);
        return instance.GetComponent<T>();
    }
}