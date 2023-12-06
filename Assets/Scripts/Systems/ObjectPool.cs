using System.Collections.Generic;
using TOM.Enemy;

namespace TOM.Utilities
{
    public enum OPType
    {
        CyberRoach,
        ToxicRoach
    }
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

        public T GetAbleObject(OPType selector)
        {
            foreach (T obj in objectList)
            {
                if (selector == OPType.CyberRoach)
                {
                    if (!((obj as UnityEngine.GameObject).GetComponent<CyberRoach>().IsAlive()))
                    {
                        lastObjectIndex = objectList.IndexOf(obj);
                        return obj;

                    }
                }
                if (selector == OPType.ToxicRoach)
                {
                    if (!((obj as UnityEngine.GameObject).GetComponent<ToxicRoach>().IsAlive()))
                    {
                        lastObjectIndex = objectList.IndexOf(obj);
                        return obj;

                    }
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