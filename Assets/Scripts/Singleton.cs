using System;

public abstract class Singleton<T> : IDisposable where T : class, new()
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            CreateInstance();
            return instance;
        }
    }
    public static void CreateInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
    }

    public static bool IsExists()
    {
        return instance != null;
    }

    public virtual void Dispose()
    {
        instance= null;
    }
}
