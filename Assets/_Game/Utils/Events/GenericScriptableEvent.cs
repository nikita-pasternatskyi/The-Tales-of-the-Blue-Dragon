using System;

namespace MP.Game.Utils
{
    public abstract class GenericScriptableEvent<T> : ScriptableEvent
    {
        public event Action<T> GenericEvent;

        public virtual void Invoke(T obj)
        {
            Invoke();
            GenericEvent?.Invoke(obj);
        }
    }

    public abstract class GenericScriptableEvent<T1, T2> : ScriptableEvent
    {
        public event Action<T1, T2> GenericEvent;

        public virtual void Invoke(T1 obj1, T2 obj2)
        {
            Invoke();
            GenericEvent?.Invoke(obj1, obj2);
        }
    }
}
