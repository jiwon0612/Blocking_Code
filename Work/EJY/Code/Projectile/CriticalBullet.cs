using UnityEngine;

namespace Projectiles
{
    public class CriticalBullet : Projectile
    {
        [SerializeField] private float criticalRate = 0.5f;
        [SerializeField] private float criticalDamage = 2f;
        [SerializeField] private ParticleSystem criticalEffect;

        private float _originDamage;

        protected override void Awake()
        {
            base.Awake();
            _originDamage = damage;
        }

        public override void FireAndInit(Transform target)
        {
            base.FireAndInit(target);

            float rate = Random.value;

            if (rate > criticalRate)
            {
                criticalEffect.Play(true);
                damage *= criticalDamage;
            }
        }

        public override void ResetItem()
        {
            base.ResetItem();
            criticalEffect.Stop(true);
            damage = _originDamage;
        }
    }
}