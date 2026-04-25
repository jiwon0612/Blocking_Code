using System;
using UnityEngine;

namespace Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityCompo
    {
        public event Action OnAnimationEndTrigger;
        public event Action<bool> OnDamageToggleTrigger;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        public void StartAttack() => OnDamageToggleTrigger?.Invoke(true);
        public void EndAttack() => OnDamageToggleTrigger?.Invoke(false);
    }
}