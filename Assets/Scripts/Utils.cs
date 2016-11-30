﻿using UnityEngine;
using System.Collections;
using System;

public static class Utils
{
    public static Vector3 tileSizeInUnits = new Vector3(1.0f, 0.57f, 0.5f);
    public static float minFloat = 0.05f;

    public static Vector3 toIsometric(Vector3 localPosition)
    {
        float isoX = (localPosition.x - localPosition.y) * tileSizeInUnits.x / 2f;
        float isoY = (localPosition.x + localPosition.y) * tileSizeInUnits.y / 2f;
        return new Vector3(isoX, isoY, isoY);
    }

    public static Vector3 toIsometric(Cart localPosition)
    {
        float isoX = (localPosition.x - localPosition.y) * tileSizeInUnits.x / 2f;
        float isoY = (localPosition.x + localPosition.y) * tileSizeInUnits.y / 2f;
        return new Vector3(isoX, isoY, isoY);
    }

    public static Vector3 toCartesianVector3(Vector3 isoPosition)
    {
        isoPosition.x = (float)Math.Round(isoPosition.x, 2);
        isoPosition.y = (float)Math.Round(isoPosition.y, 2);
        float cartx = (isoPosition.x * (2f / tileSizeInUnits.x) + isoPosition.y * (2f / tileSizeInUnits.y));
        cartx = cartx < minFloat && cartx > -1 ? 0f : cartx / 2f;
        float carty = (isoPosition.y * (2f / tileSizeInUnits.y) - cartx);
        carty = carty < minFloat && carty > -1 ? 0f : carty;
        return new Vector3(cartx, carty);
    }

    public static Cart toCartesian(Vector3 isoPosition)
    {
        isoPosition.x = (float)Math.Round(isoPosition.x, 2);
        isoPosition.y = (float)Math.Round(isoPosition.y, 2);
        float cartx = (isoPosition.x * (2f / tileSizeInUnits.x) + isoPosition.y * (2f / tileSizeInUnits.y));
        cartx = cartx < minFloat && cartx > -1 ? 0f : cartx / 2f;
        float carty = (isoPosition.y * (2f / tileSizeInUnits.y) - cartx);
        carty = carty < minFloat && carty > -1 ? 0f : carty;
        return new Cart((int)Mathf.Round(cartx), (int)Mathf.Round(carty));
    }

    public static Vector3 roundVector3(Vector3 vec)
    {
        vec.x = Mathf.Round(vec.x);
        vec.y = Mathf.Round(vec.y);
        return vec;
    }

    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }
}

