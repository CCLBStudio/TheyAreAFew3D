using CCLBStudio.GlobalUpdater;
using CCLBStudio.ScriptablePooling;
using Gameplay.Stratagem.Core;
using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerStratagemCaller : MonoBehaviour, IScriptablePooledObject, IFixedUpdatable
    {
        [SerializeField] private float throwForce = 15f;
        [SerializeField] private GameObject signal;
        
        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }

        private Rigidbody _rb;
        private float _timer;
        private bool _calledStratagem = false;
        private IRuntimeStratagem _stratagem;
        private Vector3 _userPosAtThrow;

        public void OnObjectCreated()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void OnObjectRequested()
        {
            signal.SetActive(false);
            _timer = 0f;
            _calledStratagem = false;
            GlobalUpdater.RegisterFixedUpdatable(this);
        }

        public void OnObjectReleased()
        {
            GlobalUpdater.UnregisterFixedUpdatable(this);
        }

        public void Throw(Vector3 playerPosAtThrow, IRuntimeStratagem stratagem)
        {
            _userPosAtThrow = playerPosAtThrow;
            _stratagem = stratagem;
            Vector3 rotatedForward = Quaternion.AngleAxis(-45f, transform.right) * transform.forward;
            _rb.AddForce(rotatedForward * throwForce, ForceMode.VelocityChange);
        }

        public void FixedTick()
        {
            if (_calledStratagem)
            {
                return;
            }
            
            if (_rb.linearVelocity.magnitude <= .01f)
            {
                _timer += Time.fixedDeltaTime;
                if (!(_timer >= 1f))
                {
                    return;
                }
                
                signal.SetActive(true);
                _calledStratagem = true;
                var ctx = new DefaultStratagemLaunchContext(_userPosAtThrow, transform.position);
                _stratagem.Launch(ctx);
            }
            else
            {
                _timer = 0f;
            }
        }
    }
}
