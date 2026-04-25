using System;
using Core.ObjectPool.Runtime;
using Enemies;
using UnityEngine;
using UnityEngine.Events;
using Work.CJW.Code.ETC;

namespace Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public UnityEvent OnFireEvent;
        public UnityEvent OnDestroyEvent;
        public UnityEvent OnHitEvent;

        [SerializeField] protected float damage;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected PoolingItemSO hitEffect;
        [SerializeField] protected PoolManagerSO poolManager;

        protected bool _isFire;
        protected Transform _target;

        protected float _currentLifeTime = 0;

        [field: SerializeField] public PoolingItemSO PoolingType { get; protected set; }
        public GameObject GameObject => gameObject;

        protected Pool _myPool;

        protected Rigidbody _rigidCompo;

        protected virtual void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            if (!_isFire) return;

            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime >= lifeTime)
            {
                BulletDead();
            }

            if (_target != null)
            {
                if (!_target.gameObject.activeInHierarchy)
                {
                    _target = null;
                    return;
                }

                Vector3 point1 = _target.position;
                Vector3 point2 = transform.position;
                point1.y = 0;
                point2.y = 0;

                _rigidCompo.linearVelocity = (point1 - point2).normalized * moveSpeed;
                transform.rotation = Quaternion.LookRotation(_rigidCompo.linearVelocity);
            }
        }

        public virtual void BulletDead()
        {
            if (hitEffect != null)
            {
                EffectPlayer player = poolManager.Pop(hitEffect) as EffectPlayer;
                player.PlayEffect(transform.position);
            }

            OnDestroyEvent?.Invoke();
            _target = null;
            _isFire = false;
            _myPool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void ResetItem()
        {
            _currentLifeTime = 0;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                OnHitEvent?.Invoke();
                BulletDead();
            }
        }

        public virtual void FireAndInit(Transform target)
        {
            OnFireEvent?.Invoke();
            _target = target;
            _isFire = true;
        }
    }
}