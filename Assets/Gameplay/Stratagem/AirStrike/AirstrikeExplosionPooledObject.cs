using CCLBStudio.GlobalUpdater;
using CCLBStudio.ScriptablePooling;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Gameplay.Stratagem.AirStrike
{
    public class AirstrikeExplosionPooledObject : MonoBehaviour, IScriptablePooledObject, IUpdatable
    {
        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }
        
        private MMF_Player _feedback;
    
        private ParticleSystem _system;

        public void PlayParticles()
        {
            if (!_system)
            {
                Debug.LogError("No particle system on objet !");
                GlobalUpdater.UnregisterUpdatable(this);
                Pool.ReleaseObject(this);
            }

            _system.Play(true);
            _feedback?.PlayFeedbacks();
            GlobalUpdater.RegisterUpdatable(this);
        }
    
        public void OnObjectCreated()
        {
            _system = GetComponentInChildren<ParticleSystem>();
            _feedback = GetComponentInChildren<MMF_Player>();
        }

        public void OnObjectRequested()
        {
        }

        public void OnObjectReleased()
        {
            GlobalUpdater.UnregisterUpdatable(this);
        }

        private void OnDestroy()
        {
            GlobalUpdater.UnregisterUpdatable(this);
        }

        public void Tick()
        {
            if (!_system.IsAlive(true))
            {
                GlobalUpdater.UnregisterUpdatable(this);
                Pool.ReleaseObject(this);
            }
        }
    }
}
