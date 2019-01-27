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
}