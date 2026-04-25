using Core.ObjectPool.Runtime;
using Enemies;
using UnityEngine;
using Work.CJW.Code.ETC;

namespace Projectiles
{
    public class ChainLightning : Projectile
    {
        [SerializeField] private int maxChainingCount;
        [SerializeField] private float chainingRadius;
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private LightningEffect effect;
        [SerializeField] private PoolingItemSO lightningEffect;

        private LightningEffect[] lineRenderers;

        private Collider[] _result;

        protected override void Awake()
        {
            base.Awake();
            _result = new Collider[maxChainingCount];
            lineRenderers = new LightningEffect[maxChainingCount - 1];
            for (int i = 0; i < maxChainingCount - 1; i++)
            {
                lineRenderers[i] = Instantiate(effect);
                lineRenderers[i].Init();
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            int cnt = Physics.OverlapSphereNonAlloc(transform.position, maxChainingCount, _result,
                whatIsTarget.value);

            for (int i = 0; i < cnt; i++)
            {
                if (_result[i].TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                    OnHitEvent?.Invoke();
                    
                    EffectPlayer effect = poolManager.Pop(lightningEffect) as EffectPlayer;
                    effect.PlayEffect(enemy.transform.position);
                    
                    if (i != 0)
                    {
                        lineRenderers[i - 1]
                            .Connection(_result[i - 1].transform, _result[i].transform);
                    }
                }
            }

            BulletDead();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chainingRadius);
        }
    }
}