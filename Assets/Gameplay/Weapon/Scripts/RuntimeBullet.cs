using CCLBStudio.ScriptablePooling;
using UnityEngine;

namespace Gameplay.Weapon
{
    public class RuntimeBullet : MonoBehaviour, IScriptablePooledObject//, IDamageDealer
    {
        public Transform ObjectTransform => transform;
        public ScriptablePool Pool { get; set; }
        public Vector3 Direction { get; set; }

        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider bulletCollider;

        private bool _isAlive;
        private bool _isInit;
        private float _currentTravelDistance;
        private RuntimeWeapon _currentWeapon;

        void FixedUpdate()
        {
            if(!_isAlive || !_isInit)
            {
                return;
            }

            Vector3 delta = transform.forward * (_currentWeapon.ComputeBulletSpeed() * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + delta);
            _currentTravelDistance += delta.magnitude;

            if (_currentTravelDistance >= _currentWeapon.ComputeAttackRange())
            {
                Pool.ReleaseObject(this);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_isAlive)
            {
                return;
            }
            
            Debug.Log($"Collided with {other.gameObject.name}");

            _isAlive = false;
        
            Pool.ReleaseObject(this);
            
            // var interactors = other.gameObject.GetComponents<IDamageable>();
            // if (interactors.Length <= 0)
            // {
            //     var effect = _currentWeapon.GroundImpactPool.RequestObjectAs<PooledParticleSystem>();
            //     effect.transform.position = other.ClosestPoint(transform.position);
            //     effect.Play();
            //     Pool.ReleaseObject(this);
            //     return;
            // }
            //
            // foreach (var i in interactors)
            // {
            //     i.GetHit(this);
            // }
            //
            // Pool.ReleaseObject(this);
        }

        public void Initialize(RuntimeWeapon weapon)
        {
            _currentWeapon = weapon;
            _currentTravelDistance = 0f;
            _isInit = true;

            if (!bulletCollider)
            {
                bulletCollider = GetComponentInChildren<Collider>();
            }
            
            if (!rb)
            {
                rb = GetComponentInChildren<Rigidbody>();
            }
        }

        public void OnObjectCreated()
        {
            _isAlive = false;
            _isInit = false;
        }

        public void OnObjectReleased()
        {
            _isAlive = false;
            _isInit = false;
        }

        public void OnObjectRequested()
        {
            _currentTravelDistance = 0f;
            _isAlive = true;
        }
    }
}
