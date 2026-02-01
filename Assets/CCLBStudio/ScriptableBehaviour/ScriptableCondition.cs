using UnityEngine;

namespace CCLBStudio.ScriptableBehaviour
{
    public abstract class ScriptableCondition : ScriptableObject
    {
        public abstract bool Check();
    }
}
