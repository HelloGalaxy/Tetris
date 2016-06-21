using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TransformExtension
{

    public static void DestroyAllChildren(this Transform trans)
    {
        var childrenObject = new List<GameObject>();

        foreach (Transform t in trans)
        {
            childrenObject.Add(t.gameObject);
        }

        childrenObject.ForEach(c => GameObject.DestroyImmediate(c));
    }
}
