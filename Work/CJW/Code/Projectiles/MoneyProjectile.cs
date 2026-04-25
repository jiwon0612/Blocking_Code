using Enemies;
using UI;
using UnityEngine;

namespace Projectiles
{
    public class MoneyProjectile : Projectile
    {
        [SerializeField] private ResourceModelSO playerInfo;
        [SerializeField] private int addMoneyValue;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                OnHitEvent?.Invoke();
                if (enemy.TakeDamage(damage))
                {
                    playerInfo.AddGold(addMoneyValue);
                }
                BulletDead();
            }
        }
    }
}