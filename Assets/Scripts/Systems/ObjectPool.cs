
using System.Collections.Generic;
using TOM.Enemy;
using UnityEditor;
using UnityEngine;

namespace TOM.Utilities
{
    public class ObjectPool<T> where T : new()
    {
        private List<T> objectList;
        private T _example;
        private int lastObjectIndex;

        public ObjectPool(int initialSize)
        {
            objectList = new List<T>();
            for (int i = 0; i < initialSize; i++)
            {
                T obj = new T();
                objectList.Add(obj);
            }
        }
        public ObjectPool()
        {
            objectList = new List<T>();
        }

        public ObjectPool(T example)
        {
            objectList = new List<T>();
            _example = example;
        }

        public T GetAbleObject()
        {
            foreach (T obj in objectList)
            {
                if (!((obj as UnityEngine.GameObject).GetComponent<CyberRoach>().IsAlive()))
                {
                    lastObjectIndex = objectList.IndexOf(obj);
                    return obj;
                }
            }

            T newObj;

            if (_example != null)
            {
                newObj = _example;
            }
            else
            {
                newObj = new T();
            }
            objectList.Add(newObj);
            lastObjectIndex = objectList.IndexOf(newObj);
            return newObj;
        }

        public T GetT() => _example;
        public int GetPoolCount() => objectList.Count;

        public void UpdateLastGivenObject(T newObject)
        {
            objectList[lastObjectIndex] = newObject;
        }

    }

}