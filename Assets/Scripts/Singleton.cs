using System;

namespace Chat
{
    public abstract class Singleton<T> : IDisposable where T : class, new()
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                CreateInstance();
                return _instance;
            }
        }
        public static void CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }
        }

        public static bool IsExists()
        {
            return _instance != null;
        }

        public virtual void Dispose()
        {
            _instance= null;
        }
    }
}
