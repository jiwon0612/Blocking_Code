using Entities;

namespace Code.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected int _animationHash;
        protected EntityAnimator _animator;
        protected EntityAnimatorTrigger _animatorTrigger;

        protected bool _isTriggerCall;

        public EntityState(Entity entity, int animationHash)
        {
            _entity = entity;
            _animationHash = animationHash;
            _animator = _entity.GetCompo<EntityAnimator>();
            _animatorTrigger = _entity.GetCompo<EntityAnimatorTrigger>();
        }

        public virtual void Enter()
        {
            _animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
            _animator.SetParam(_animationHash, true);
            _isTriggerCall = false;
        }
        
        public virtual void Update() {}

        public virtual void Exit()
        {
            _animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
            _animator.SetParam(_animationHash, false);
        }
        
        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}