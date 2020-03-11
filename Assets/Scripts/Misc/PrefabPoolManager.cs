using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class PrefabPoolManager<T> where T : MonoBehaviour
    {
        private Dictionary<int, MonoPool<T>> poolCollection = new Dictionary<int, MonoPool<T>>();
        
        public MonoPool<T> CreatePool(Transform parent, T prefab)
        {
            var pool = new MonoPool<T>(parent, prefab);
            poolCollection[prefab.GetInstanceID()] = pool;
            return pool;
        }

        public MonoPool<T> GetPool(T prefab)
        {
            var id = prefab.GetInstanceID();
            
            return poolCollection.ContainsKey(id) ? poolCollection[id] : null;
        }

        public void Clear()
        {
            foreach (var pool in poolCollection)
            {
                pool.Value.Dispose();
            }
            poolCollection.Clear();
        }
    }
}