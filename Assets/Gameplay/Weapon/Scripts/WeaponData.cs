using CCLBStudio.ScriptablePooling;
using UnityEngine;

namespace Gameplay.Weapon
{
    [CreateAssetMenu(menuName = "Gameplay/Weapon/Data", fileName = "NewWeapon")]
    public class WeaponData : ScriptableObject
    {
        public float AttackDamages => attackDamages;
        public float AttackSpeed => attackSpeed;
        public float AttackRange => attackRange;
        public float BulletSpeed => bulletSpeed;
        public float Dispersion => dispersion;
        public RuntimeWeapon WeaponPrefab => weaponPrefab;
        public ScriptablePool BulletPool => bulletPool;
        public ScriptablePool CasingPool => casingPool;
        public ScriptablePool MuzzlePool => muzzlePool;
        public ScriptablePool ImpactPool => impactPool;

        [Header("Weapon Visuals")]
        [Tooltip("Weapon model")]
        [SerializeField] private RuntimeWeapon weaponPrefab;
        [SerializeField] private ScriptablePool bulletPool;
        [SerializeField] private ScriptablePool casingPool;
        [SerializeField] private ScriptablePool muzzlePool;
        [SerializeField] private ScriptablePool impactPool;
    
        [Header("Weapon Stats")]
        [Tooltip("Base weapon damages")]
        [SerializeField] private float attackDamages;
        [Tooltip("Base attacks per second.")]
        [SerializeField] private float attackSpeed;
        [Tooltip("Base weapon range")]
        [SerializeField] private float attackRange;
        [Tooltip("Base bullet speed")]
        [SerializeField] private float bulletSpeed;
    
        [Header("Bullet Stats")]
        [Tooltip("Base dispersion range for each bullet. Dispersion is a random angle between in range [-dispersion : dispersion] added to the shooting direction.")]
        [SerializeField] private float dispersion;


        public RuntimeWeapon Equip(IWeaponHolder holder)
        {
            var weapon = Instantiate(weaponPrefab, holder.GetWeaponPivot(this));
            bulletPool?.Initialize();
            casingPool?.Initialize();
            muzzlePool?.Initialize();
            impactPool?.Initialize();

            weapon.Initialize(holder, this);

            return weapon;
        }
        
        public RuntimeBullet RequestBullet() => bulletPool.RequestObjectAs<RuntimeBullet>();
        public PooledParticleSystem RequestMuzzle() => muzzlePool.RequestObjectAs<PooledParticleSystem>();
    }
}
