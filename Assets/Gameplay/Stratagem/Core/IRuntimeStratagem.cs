namespace Gameplay.Stratagem.Core
{
    public interface IRuntimeStratagem
    {
        public void Launch(in DefaultStratagemLaunchContext ctx);
        public IRuntimeStratagem Initialize(StratagemData stratagemData, IStratagemHolder stratagemHolder);
    }
}
