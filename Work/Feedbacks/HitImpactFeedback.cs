using Core.Dependencies;
using Core.ObjectPool.Runtime;
using DG.Tweening;
using UnityEngine;
using Work.Effects;

namespace Work.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private PoolingItemSO hitImpactItem;
        [SerializeField] private float playDuration = 0.5f;

        [Inject] private PoolManagerMono _poolManager;

        private PoolingEffect _effect;
        
        public override void CreateFeedback()
        {
            _effect = _poolManager.Pop<PoolingEffect>(hitImpactItem);
            _effect.PlayVFX(transform.position, Quaternion.identity,playDuration);
        }

        public override void StopFeedback()
        { }
    }
}