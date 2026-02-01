using UnityEngine;

namespace CCLBStudio.ScriptableBehaviour
{
    public abstract class ScriptableStrategy<T> : ScriptableObject
    {
        public abstract void Execute(T parameter);
    }
}
