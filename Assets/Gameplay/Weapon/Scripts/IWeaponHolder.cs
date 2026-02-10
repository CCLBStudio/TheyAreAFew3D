using UnityEngine;

namespace Gameplay.Weapon
{
    public interface IWeaponHolder
    {
        public Transform GetWeaponPivot(WeaponData weapon);
        public float GetAttackRangeMultiplier() => 1f;
        public float GetAttackDamagesMultiplier() => 1f;
        public float GetAttackSpeedMultiplier() => 1f;
        public float GetBulletSpeedMultiplier() => 1f;
    }
}
