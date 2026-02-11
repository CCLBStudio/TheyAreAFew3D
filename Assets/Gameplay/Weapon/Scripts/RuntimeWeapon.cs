using CCLBStudio.ScriptablePooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapon
{
    public class RuntimeWeapon : MonoBehaviour
    {
        [SerializeField] protected Transform bulletOrigin;
        [SerializeField] protected Transform muzzleOrigin;
        
        protected IWeaponHolder holder;
        protected WeaponData weaponData;

        private bool _init;
        private bool _shooting;
        private float _timer;

        protected virtual void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
                return;
            }

            if (!_shooting)
            {
                return;
            }
            
            Shoot();
        }

        public virtual void Initialize(IWeaponHolder holder, WeaponData weaponData)
        {
            this.holder = holder;
            this.weaponData = weaponData;

            _init = true;
        }

        public virtual void StartShooting()
        {
            _shooting = true;
        }

        public virtual void StopShooting()
        {
            _shooting = false;
        }

        protected virtual void Shoot()
        {
            _timer = 1f / ComputeAttackSpeed();
            
            if (!_init)
            {
                return;
            }
        
            SpawnBullet();
        }

        protected void SpawnBullet()
        {
            var bullet = weaponData.RequestBullet();
            bullet.Initialize(this);
            bullet.transform.position = bulletOrigin.position;
            
            Vector3 euler = bulletOrigin.eulerAngles;
            euler.x = 0;
            float dispersion = Random.Range(-weaponData.Dispersion, weaponData.Dispersion);
            Vector3 direction = ApplyRotation(euler, Vector3.up, dispersion);
            bullet.Direction = direction;
            bullet.transform.rotation = Quaternion.Euler(direction);
        }

        protected PooledParticleSystem SpawnMuzzle()
        {
            var muzzle = weaponData.RequestMuzzle()
                .Parent(muzzleOrigin)
                .AtLocal(Vector3.zero)
                .WithLocalRot(Quaternion.identity);

            return muzzle;
        }

        private Vector3 ApplyRotation(Vector3 direction, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * direction;
        }

        public float ComputeAttackSpeed() => weaponData.AttackSpeed * holder.GetAttackSpeedMultiplier();
        public float ComputeAttackRange() => weaponData.AttackRange * holder.GetAttackRangeMultiplier();
        public float ComputeBulletSpeed() => weaponData.BulletSpeed * holder.GetBulletSpeedMultiplier();

    }
}
