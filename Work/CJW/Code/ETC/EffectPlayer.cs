using System;
using Core.ObjectPool.Runtime;
using DG.Tweening;
using UnityEngine;

namespace Work.CJW.Code.ETC
{
    public class EffectPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private float duration;
        
        private ParticleSystem _particle;
        private Pool _myPool;
        
        [field:SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _particle.Stop();
        }

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void PlayEffect(Vector3 position)
        {
            transform.position = position;
            _particle.Play();
            DOVirtual.DelayedCall(duration, () => _myPool.Push(this));
        }
    }
}