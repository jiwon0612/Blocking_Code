using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class DebuffProjectile : Projectile
    {
        [SerializeField] private float debuffValue;
        [SerializeField] private float debuffDuration;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.AddBuff(EnemyBuffReason.SlowerTower, -debuffValue,debuffDuration);
            }
        }
    }
}