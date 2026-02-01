using UnityEngine;

namespace CCLBStudio.ScriptableBehaviour
{
    public abstract class ScriptableOperationStrategy<T, TR> : ScriptableObject
    {
        public abstract TR Execute(T parameter);
    }
}
