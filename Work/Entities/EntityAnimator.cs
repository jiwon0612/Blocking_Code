using UnityEngine;

namespace Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityCompo
    {
        protected Animator _animator;
        protected Entity _entity;
        
        public virtual void Initialize(Entity entity)
        {
            _animator = GetComponent<Animator>();
        }
        
        public void SetParam(int paramHash,int value) => _animator.SetInteger(paramHash, value);
        public void SetParam(int paramHash, float value) => _animator.SetFloat(paramHash, value);
        public void SetParam(int paramHash, bool value) => _animator.SetBool(paramHash, value);
        public void SetParam(int paramHash) => _animator.SetTrigger(paramHash);
    }
}