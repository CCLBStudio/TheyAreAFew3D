using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    public struct AirstrikeLaunchContext
    {
        public AirstrikeStratagemData StratagemData { get; private set; }
        public Vector3 UnitPositionAtLaunch { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public AirstrikeLaunchContext(DefaultStratagemLaunchContext baseContext, AirstrikeStratagemData data)
        {
            UnitPositionAtLaunch = baseContext.UnitPositionAtLaunch;
            TargetPosition = baseContext.TargetPosition;
            StratagemData = data;
        }
    }
}