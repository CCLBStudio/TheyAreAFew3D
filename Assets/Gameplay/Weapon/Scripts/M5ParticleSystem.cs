using CCLBStudio.ScriptablePooling;
using UnityEngine;

namespace Gameplay.Weapon
{
    public sealed class M5ParticleSystem : PooledParticleSystem, IStatBasedParticleSystem
    {
        [SerializeField] private ParticleSystem muzzle;
        [SerializeField] private ParticleSystem smoke;
        
        public void SetForStats(RuntimeWeapon weapon)
        {
            var muzzleEmission = muzzle.emission;
            muzzleEmission.rateOverTime = weapon.ComputeAttackSpeed();
            
            var smokeEmission = smoke.emission;
            smokeEmission.rateOverTime = weapon.ComputeAttackSpeed();
        }
    }
}
