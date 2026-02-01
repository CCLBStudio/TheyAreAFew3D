using UnityEngine;

namespace Gameplay.Stratagem.Core
{
    public struct DefaultStratagemLaunchContext
    {
        public DefaultStratagemLaunchContext(Vector3 unitPositionAtLaunch, Vector3 targetPosition)
        {
            UnitPositionAtLaunch = unitPositionAtLaunch;
            TargetPosition = targetPosition;
        }

        public Vector3 UnitPositionAtLaunch { get; }
        public Vector3 TargetPosition { get; }
    }
}
