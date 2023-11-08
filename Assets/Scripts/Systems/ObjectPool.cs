using System.Collections.Generic;

public class ObjectPool<T> where T : new()
{
    private List<T> objectList;

    public ObjectPool(int initialSize)
    {
        objectList = new List<T>();
        for (int i = 0; i < initialSize; i++)
        {
            T obj = new T();
            objectList.Add(obj);
        }
    }

    public T GetAbleObject()
    {
        foreach (T obj in objectList)
        {
            object isActiveValue = obj.GetType().GetProperty("isActive")?.GetValue(obj);
            bool aux = isActiveValue != null && isActiveValue.Equals(false);

            if (aux)
            {
                obj.GetType().GetProperty("isActive")?.SetValue(obj, true);
                return obj;
            }
        }
        
        T newObj = new T();
        newObj.GetType().GetProperty("IsActive")?.SetValue(newObj, true);
        objectList.Add(newObj);
        return newObj;
    }
}
