using Gameplay.Stratagem.Core;

namespace Gameplay.Stratagem.AirStrike
{
    public class AirstrikeRuntimeStratagem : RuntimeStratagem<AirstrikeLaunchContext, AirstrikeStratagemData>
    {
        protected override void Setup()
        {
        }

        protected override void PlayEffects(in AirstrikeLaunchContext ctx)
        {
            foreach (var effect in effects)
            {
                effect.LaunchEffect(ctx);
            }
        }

        public override void Launch(in DefaultStratagemLaunchContext ctx)
        {
            AirstrikeLaunchContext airstrikeContext = new AirstrikeLaunchContext(ctx, data);
            PlayEffects(airstrikeContext);
        }
    }
}