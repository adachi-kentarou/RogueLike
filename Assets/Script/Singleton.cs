using System;
public abstract class Singleton<T> where T : class, new ()
{
    protected Singleton()
    {

    }

    private static readonly T _instance = new T();

    public static T Instance {
        get
        {
            return _instance;
        }
    }
}
