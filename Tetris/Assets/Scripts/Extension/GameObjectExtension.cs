using System;
using System.Collections.Generic;
using UnityEngine;


public static class GameObjectExtension
{
    public static GameObject CreateNewGameObject(this GameObject prefab, Transform parent, string name)
    {
        var newGameObject = new GameObject();
        newGameObject.transform.parent = parent;
        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.name = name;

        return newGameObject;
    }

    public static GameObject CloneGameObjectFromSelf(this GameObject prefab, Transform parent, string name, Vector3 pos)
    {
        var newGameObject = GameObject.Instantiate(prefab);
        newGameObject.transform.parent = parent;
        newGameObject.transform.localPosition = pos;
        newGameObject.name = name;

        return newGameObject;
    }
}

