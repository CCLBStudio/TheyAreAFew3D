using CCLBStudio.GlobalUpdater;
using UnityEngine;

namespace CCLBStudio.ScriptablePooling
{
    public class PooledParticleSystem : MonoBehaviour, IScriptablePooledObject, IUpdatable
    {
        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }

        [SerializeField] private ParticleSystem system;

        private bool _checkForRelease;
        private Quaternion _initialRotation;
    
        public void Tick()
        {
            if (!_checkForRelease)
            {
                return;
            }

            if (!system.IsAlive())
            {
                Pool.ReleaseObject(this);
            }
        }

        public void Play()
        {
            if (!system)
            {
                Pool.ReleaseObject(this);
                return;
            }

            system.Play(true);
            _checkForRelease = true;
        }

        public void Stop(ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting)
        {
            if (!system)
            {
                Pool.ReleaseObject(this);
                return;
            }

            system.Stop(true, stopBehavior);
        }
    
        public void OnObjectCreated()
        {
            if (!system)
            {
                system = GetComponent<ParticleSystem>();
                if (!system)
                {
                    Debug.LogError("No particle system on objet !");
                }
            }

            _initialRotation = transform.rotation;
        }

        public void OnObjectRequested()
        {
            GlobalUpdater.GlobalUpdater.RegisterUpdatable(this);
        }

        public void OnObjectReleased()
        {
            GlobalUpdater.GlobalUpdater.UnregisterUpdatable(this);
        
            _checkForRelease = false;
            transform.position = Vector3.zero;
            transform.rotation = _initialRotation;
        }
    }
}
