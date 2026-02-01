using UnityEngine;

namespace CCLBStudio.ScriptableBehaviour
{
    public abstract class ScriptableOperation<T> : ScriptableObject
    {
        public abstract T Execute();
    }
}
