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
            if (_muzzle is IStatBasedParticleSystem p)
            {
                p.SetForStats(this);
            }
            _muzzle.Play();
        }
        
        public override void StopShooting()
        {
            base.StopShooting();
            _muzzle?.Stop();
        }
    }
}
