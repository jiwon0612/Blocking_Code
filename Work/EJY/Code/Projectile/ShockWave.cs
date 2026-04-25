using System;
using System.Collections.Generic;
using Core.ObjectPool.Runtime;
using DG.Tweening;
using Enemies;
using UnityEngine;
using Work.CJW.Code.ETC;

namespace Projectiles
{
    public class ShockWave : MonoBehaviour, IPoolable
    {
        [SerializeField] private float damage;
        [SerializeField] private float duration;
        [SerializeField] private float maxSize = 1.5f;
        [SerializeField] private ParticleSystem shockWaveEffect;
        [SerializeField] private PoolingItemSO hitEffect;
        [SerializeField] private PoolManagerSO poolManager;
                
        private Pool _myPool;
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;

        [ContextMenu("테스트")]
        public void Test() => InitShockWave(transform.position);
        public void InitShockWave(Vector3 pos)
        {
            IEnumerable<ParticleSystem> particles = shockWaveEffect.transform.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.startLifetime = duration;
            }
            
            shockWaveEffect.Play(true);
            
            transform.position = pos;
            transform.localScale = Vector3.zero;
            transform.DOScale(maxSize, duration)
                .OnComplete(() => _myPool.Push(this));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                EffectPlayer player = poolManager.Pop(hitEffect) as EffectPlayer;
                player.PlayEffect(enemy.transform.position);
            }
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
        }
    }
}