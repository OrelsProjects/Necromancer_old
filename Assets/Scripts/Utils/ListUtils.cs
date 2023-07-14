using System.Collections.Generic;
using UnityEngine;
public class ListUtils
{
    public static void AddUnique<T>(ref List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }
}