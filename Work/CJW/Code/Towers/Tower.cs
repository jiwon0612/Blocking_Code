using System;
using System.Collections.Generic;
using Core.ObjectPool.Runtime;
using Enemies;
using Entities;
using UnityEngine;
using Projectiles;
using UnityEngine.Events;
using Work.JES.Code;
using Work.JES.Code.SynergySystems;

namespace Towers
{
    public class Tower : Entity, IPoolable
    {
        [SerializeField] protected Transform firePosTrm;
        [SerializeField] protected PoolManagerSO poolManager;
        [SerializeField] protected float attackDistance;
        [SerializeField] protected float attackDelay;
        [SerializeField] protected LayerMask whatIsTarget;
        [SerializeField] protected PoolingItemSO bulletPoolType;
        [SerializeField] protected int maxFindEnemyCount;
        
        [field:SerializeField] public MeshFilter MeshFilter { get; private set; }
        
        [field:SerializeField] public SunergyType SynergyType { get; private set; } = SunergyType.Red;
        public TowerSynergy synergyObj=null;
        public float AttackDistance => attackDistance;
        public UnityEvent<Tower> TowerDestroyedEvent;
        protected float _currentAttackDelay;
        
        public bool IsEnabled { get; set; }
        
        protected Collider[] _result;
        protected float _timer;

        protected override void InitComponents()
        {
            base.InitComponents();
            _timer = 0;
            _result = new Collider[maxFindEnemyCount];

            _currentAttackDelay = attackDelay;
        }

        [ContextMenu("Init")]
        public virtual void Init()
        {
            IsEnabled = true;
        }

        public virtual void DestroyTower()
        {
            IsEnabled = false;
            TowerDestroyedEvent?.Invoke(this);
            _myPool.Push(this);
        }

        public void SetAttackDelayValue(float value)
        {
            _currentAttackDelay = attackDelay / value;
        }

        protected virtual void Update()
        {
            if (!IsEnabled) return;
            
            _timer += Time.deltaTime;
            if (_timer >= attackDelay)
            {
                _timer = 0;
                Shooting(FindEnemy());
            }
        }

        public Transform FindEnemy()
        {
            int cnt = Physics.OverlapSphereNonAlloc(transform.position, attackDistance, _result, whatIsTarget.value);

            float minDistance = int.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < cnt; i++)
            {
                if (Vector3.Distance(transform.position, _result[i].transform.position) <= minDistance)
                {
                    if (_result[i].TryGetComponent(out Enemy enemy))
                    {
                        minDistance = Vector3.Distance(transform.position, _result[i].transform.position);
                        minIndex = i;
                    }
                }
            }

            if (minIndex != -1)
                return _result[minIndex].transform;
            
            return null;
        }

        public virtual void Shooting(Transform target)
        {
            if (target == null)
                return;

            Projectile bullet = poolManager.Pop(bulletPoolType) as Projectile;
            bullet.transform.position = firePosTrm.position;
            bullet.transform.rotation = Quaternion.LookRotation(target.position - firePosTrm.position);
            bullet.FireAndInit(target);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }

        [field:SerializeField] public PoolingItemSO PoolingType { get;private set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;
        
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public virtual void ResetItem()
        {
            
        }
    }
}