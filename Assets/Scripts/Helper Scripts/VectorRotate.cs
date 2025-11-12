using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float rads = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rads);
        float cos = Mathf.Cos(rads);

        float vx = v.x;
        float vy = v.y;

        v.x = (cos * vx) - (sin * vy);
        v.y = (sin * vx) + (cos * vy);

        return v;
    }
}