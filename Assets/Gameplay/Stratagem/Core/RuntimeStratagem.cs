using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Stratagem.Core
{
    public abstract class RuntimeStratagem<TContext, TData> : MonoBehaviour, IRuntimeStratagem where TData : StratagemData
    {
        [SerializeReference] protected List<StratagemEffect<TContext>> effects;
        
        protected TData data;
        protected IStratagemHolder holder;

        protected abstract void Setup();
        protected abstract void PlayEffects(in TContext ctx);
        public abstract void Launch(in DefaultStratagemLaunchContext ctx);

        public IRuntimeStratagem Initialize(StratagemData stratagemData, IStratagemHolder stratagemHolder)
        {
            if (stratagemData is not TData specificData)
            {
                Debug.LogError($"{GetType().Name} requires data of type {typeof(TData).Name}");
                return null;
            }
            
            data = specificData;
            holder = stratagemHolder;
            Setup();
            return this;
        }
    }
}
