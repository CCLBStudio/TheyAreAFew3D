using System;

namespace Gameplay.Stratagem.Core
{
    [Serializable]
    public abstract class StratagemEffect<TContext>
    {
        public abstract void LaunchEffect(in TContext context);
    }
}
