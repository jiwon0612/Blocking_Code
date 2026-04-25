using Core.ObjectPool.Runtime;
using Enemies;
using UnityEngine;
using UnityEngine.Events;
using Work.CJW.Code.ETC;

namespace Work.CJW.Code.SynergySkills
{
    public class Mine : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolingItemSO explosionEffect;
        [SerializeField] private PoolManagerSO poolManager;

        public UnityEvent OnHitEvent;
        
        private bool _isActive;
        private float _damage;

        public void InitMaie(float damage)
        {
            _damage = damage;
            _isActive = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive)
            {
                if (other.TryGetComponent(out Enemy enemy))
                {
                    EffectPlayer player = poolManager.Pop(explosionEffect) as EffectPlayer;
                    player.PlayEffect(transform.position);
                    enemy.TakeDamage(_damage);
                    OnHitEvent?.Invoke();
                    DeadMine();
                }
            }
        }

        public void DeadMine()
        {
            _isActive = false;
            _myPool.Push(this);
        }

        [field:SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {

        }
    }
}