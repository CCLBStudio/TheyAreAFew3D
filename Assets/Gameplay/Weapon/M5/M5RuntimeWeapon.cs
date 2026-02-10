using CCLBStudio.ScriptablePooling;

namespace Gameplay.Weapon.M5
{
    public class M5RuntimeWeapon : RuntimeWeapon
    {
        private PooledParticleSystem _muzzle;
        public override void StartShooting()
        {
            base.StartShooting();
            _muzzle = SpawnMuzzle();
        }
        
        public override void StopShooting()
        {
            base.StopShooting();
            _muzzle?.Stop();
        }
    }
}
