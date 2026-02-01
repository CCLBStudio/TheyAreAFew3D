using System;
using CCLBStudio.GlobalUpdater;
using Gameplay.Stratagem.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Stratagem.AirStrike
{
    [Serializable]
    public class AirstrikeExplosionEffect : StratagemEffect<AirstrikeLaunchContext>, IUpdatable
    {
        private Vector3[] _explosionsPos;
        private int _explosionIndex;
        private float _delay;
        private AirstrikeStratagemData _data;
        
        public override void LaunchEffect(in AirstrikeLaunchContext context)
        {
            GlobalUpdater.RegisterUpdatable(this);
            _data = context.StratagemData;
            _delay = _data.explosionDelay;
            _explosionIndex = 0;
            Vector3 flatDir = ComputeFlatDirection(context.TargetPosition, context.UnitPositionAtLaunch);
            _explosionsPos = ComputeCenteredPositions(context.TargetPosition, flatDir, _data.explosions, _data.distanceBetweenExplosions);
        }

        private Vector3 ComputeFlatDirection(Vector3 targetPos, Vector3 unitPos)
        {
            unitPos.y = targetPos.y;
            return (targetPos - unitPos).normalized;
        }

        public void Tick()
        {
            if (_explosionIndex >= _data.explosions)
            {
                GlobalUpdater.UnregisterUpdatable(this);
                return;
            }

            _delay -= Time.deltaTime;

            if (_delay <= 0f)
            {
                Explode();
            }
        }
        
        private void Explode()
        {
            _delay = _data.timeBetweenExplosions;
            Vector3 explosionRefPos = _explosionsPos[_explosionIndex];
            explosionRefPos.y += 20f;
            Ray ray = new Ray(explosionRefPos, _explosionsPos[_explosionIndex] - explosionRefPos);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                AirstrikeExplosionPooledObject explosion = _data.explosionPool.RequestObjectAs<AirstrikeExplosionPooledObject>();
                var t = explosion.transform;
                var p = hit.point;
                p += t.right * Random.Range(-1f, 1f);
                t.position = p;
                explosion.PlayParticles();
            }
        
            _explosionIndex++;
        }
        
        private Vector3[] ComputeCenteredPositions(Vector3 reference, Vector3 axis, int count, float spacing)
        {
            Vector3[] result = new Vector3[count];
            float center = (count - 1) * 0.5f;

            for (int i = 0; i < count; i++)
            {
                float offsetIndex = i - center;
                result[i] = reference + axis * (offsetIndex * spacing);
            }

            return result;
        }
    }
}
