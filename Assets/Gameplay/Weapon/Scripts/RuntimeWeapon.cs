using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Weapon
{
    public class RuntimeWeapon : MonoBehaviour
    {
        public float AttackSpeed => ComputeAttackSpeed();
    
        [SerializeField] private Transform bulletOrigin;
        [SerializeField] private Transform muzzleOrigin;
        
        private IWeaponHolder _holder;
        private bool _init;
        private bool _shooting;
        private float _timer;
        private WeaponData _weaponData;

        private void Update()
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

        public void Initialize(IWeaponHolder holder, WeaponData weaponData)
        {
            _holder = holder;
            _weaponData = weaponData;

            _init = true;
        }

        public void StartShooting()
        {
            _shooting = true;
        }

        public void StopShooting()
        {
            _shooting = false;
        }

        private void Shoot()
        {
            _timer = 1f / ComputeAttackSpeed();
            
            if (!_init)
            {
                return;
            }
        
            SpawnBullet();
            //SpawnMuzzle();
        }

        private void SpawnBullet()
        {
            var bullet = _weaponData.RequestBullet();
            bullet.Initialize(this);
            bullet.transform.position = bulletOrigin.position;
            
            Vector3 euler = bulletOrigin.eulerAngles;
            euler.x = 0;
            float dispersion = Random.Range(-_weaponData.Dispersion, _weaponData.Dispersion);
            Vector3 direction = ApplyRotation(euler, Vector3.up, dispersion);
            bullet.Direction = direction;
            bullet.transform.rotation = Quaternion.Euler(direction);
        }

        private void SpawnMuzzle()
        {
            var muzzle = _weaponData.RequestMuzzle();
            Transform muzzleTransform = muzzle.transform;
        
            muzzleTransform.SetParent(muzzleOrigin);
            muzzleTransform.localPosition = Vector3.zero;
            muzzleTransform.localRotation = Quaternion.identity;
        }

        private Vector3 ApplyRotation(Vector3 direction, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * direction;
        }

        private float ComputeAttackSpeed() => _weaponData.AttackSpeed * _holder.GetAttackSpeedMultiplier();
        public float ComputeAttackRange() => _weaponData.AttackRange * _holder.GetAttackRangeMultiplier();
        public float ComputeBulletSpeed() => _weaponData.BulletSpeed * _holder.GetBulletSpeedMultiplier();

    }
}
