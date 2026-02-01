using UnityEngine;

namespace CCLBStudio.ScriptablePooling
{
    public class TimedPooledObject : MonoBehaviour, IScriptablePooledObject
    {
        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }
    
        [SerializeField] private float displayTime = .2f;

        private float _timer;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Pool.ReleaseObject(this);
            }
        }

        public void OnObjectCreated()
        {
        }

        public void OnObjectRequested()
        {
            _timer = displayTime;
        }

        public void OnObjectReleased()
        {
        }
    }
}
