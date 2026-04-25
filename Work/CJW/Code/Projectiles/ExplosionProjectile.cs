using System;
using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class ExplosionProjectile : Projectile
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float explosionRadius;
        [SerializeField] private int maxHitCount;

        private Collider[] _result;

        protected override void Awake()
        {
            base.Awake();
            _result = new Collider[maxHitCount];
        }

        protected override void OnTriggerEnter(Collider other)
        {
            int cnt = Physics.OverlapSphereNonAlloc(transform.position,explosionRadius, _result,  whatIsTarget.value);
            for (int i = 0; i < cnt; i++)
            {
                if (_result[i].TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                }
            }
            
            BulletDead();
        }

        public override void BulletDead()
        {
            base.BulletDead();
            OnHitEvent?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,explosionRadius);
        }
    }
}