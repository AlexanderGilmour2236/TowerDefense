using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> where T : Component
{
    private readonly Stack<T> used = new Stack<T>();
    private readonly Stack<T> free = new Stack<T>();
            
    private readonly Transform parent;
    private readonly T prefab;
    public MonoPool(Transform parent, T prefab = null)
    {
        this.parent = parent;
        if (prefab != null) this.prefab = prefab;
    }

    public T GetObject()
    {
        if (free.Count == 0)
        {
            if (prefab == null)
            {
                var obj = new GameObject();
                obj.transform.SetParent(parent, false);
                var component = obj.AddComponent<T>();

                used.Push(component);
                return component;
            }
            else
            {
                var component = Object.Instantiate(prefab, parent);
                used.Push(component);
                return component;
            }
        }
        else
        {
            var obj = free.Pop();
            obj.gameObject.SetActive(true);
            used.Push(obj);
            return obj;
        }
    }

    public void ReleaseObject(T obj)
    {
        used.Pop();
        obj.gameObject.SetActive(false);
        free.Push(obj);
    }

    public void Dispose()
    {
        free.Clear();
        used.Clear();
    }
}