using System;
using CCLBStudio.GlobalUpdater;
using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    [Serializable]
    public class AirstrikeDecalEffect : StratagemEffect<AirstrikeLaunchContext>, IUpdatable
    {
        private float _speed;
        private float _lifetime;
        private AirstrikeDecalPooledObject _pooledDecal;
        private Transform _decalTransform;
        private AirstrikeStratagemData _data;
        
        public override void LaunchEffect(in AirstrikeLaunchContext context)
        {
            GlobalUpdater.RegisterUpdatable(this);
            _data = context.StratagemData;
            
            _speed = _data.spawnDistance / _data.timeToReachTarget;
            _pooledDecal = _data.decalPool.RequestObjectAs<AirstrikeDecalPooledObject>();
            _decalTransform = _pooledDecal.ObjectTransform;
            _lifetime = Mathf.Max(_data.timeToReachTarget * 2f, _pooledDecal.FeedbackLength());
            
            Vector3 unitPosition = context.UnitPositionAtLaunch;
            Vector3 targetPos = context.TargetPosition;
            unitPosition.y = targetPos.y;
            
            Vector3 offsetDir = (unitPosition - targetPos).normalized;
            Vector3 startPos = targetPos + offsetDir * _data.spawnDistance;
            Vector3 dir = targetPos - startPos;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            _decalTransform.SetPositionAndRotation(startPos, rot);
        }

        public void Tick()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0f)
            {
                GlobalUpdater.UnregisterUpdatable(this);
                _data.decalPool.ReleaseObject(_pooledDecal);
                return;
            }
            
            _decalTransform.Translate(_decalTransform.forward * (_speed * Time.deltaTime), Space.World);
        }
    }
}
