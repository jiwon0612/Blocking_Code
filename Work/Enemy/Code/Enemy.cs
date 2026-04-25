using Core.ObjectPool.Runtime;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Work.CJW.Code.ETC;
using Work.Feedbacks;
using Work.JES.Code.BuildSystems;

namespace Enemies
{
    public class Enemy : Entity, IPoolable
    {
        public Action<Enemy> OnEnemyDeadEvent;

        [SerializeField] private bool _isBoss = false;
        [SerializeField] private BossHPModelSO _bossHPModel;

        [SerializeField] private ResourceModelSO _resourceModel;
        // 여기서 가야할 타일 위치를 구함.
        [SerializeField] private MapManageSO _mapData;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO damageTextItem;
        [SerializeField] private PoolingItemSO deadEffect;
        
        private List<Material> mats;
        private float flashDuration = 0.3f;
        private Vector3Int _targetPositon => _mapData.CloseTowerPositionFromEnemyPosition(transform.position);

        // 에너미 기본 스탯정보 + 공격 이펙트등
        [SerializeField] private EnemyDataSO _enemyData;
        public float Health = 100.0f;
        public float HpMultiplier = 1.0f;
        public float Speed
        {
            get
            {
                float buffedSpeed = _speed;
                foreach (var buff in _activeBuffs)
                {
                    if (buff.Reason > EnemyBuffReason.Speed && buff.Reason < EnemyBuffReason.Speed + 100)
                    {
                        buffedSpeed += buff.Value;
                    }
                }
                return buffedSpeed;
            }
            set => _speed = value;
        }

        public PoolingItemSO PoolingType;

        public GameObject GameObject => gameObject;

        PoolingItemSO IPoolable.PoolingType => PoolingType;

        private List<EnemyBuff> _activeBuffs = new();
        private float _currentHP;
        private float _speed;
        private HitImpactFeedback hitImpactFeedback;
        private Coroutine blinkCoroutine;

        private void Start()
        {
            base.Awake();
            hitImpactFeedback = GetComponent<HitImpactFeedback>();
            Debug.Assert(hitImpactFeedback != null, "HitImpactFeedback component is missing on the Enemy GameObject.");
            mats = new List<Material>();
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                mats.Add(renderer.material);
            }
        }

        public void FixedUpdate()
        {
            if (IsDead) return;
            UpdateBuffs(Time.fixedDeltaTime);
            MoveTo(_targetPositon);
            if (Vector3.Distance(transform.position, _targetPositon) < 0.1f)
            {
                _mapData.RemoveTower(_targetPositon);
                _resourceModel.SubtractHealth(1);
                //hitImpactFeedback.CreateFeedback();
                TakeDamage(_currentHP, true);
            }
        }

        /// <summary>
        /// 기본값으로 돌리기(언젠가 스탯강화를 위해?)
        /// </summary>
        public void ResetStats()
        {
            Health = _enemyData.Health * HpMultiplier;
            _speed = _enemyData.Speed;
        }
        public void AddBuff(EnemyBuffReason reason, float value, float duration)
        {
            if (IsDead) return;

            // 이미 같은 이유로 버프가 있다면, 지속시간만 갱신.
            foreach (var buff in _activeBuffs)
            {
                if (buff.Reason == reason)
                {
                    buff.Duration = Mathf.Max(buff.Duration, duration);
                    return;
                }
            }
            _activeBuffs.Add(new EnemyBuff(reason, value, duration));
        }

        private void UpdateBuffs(float deltaTime)
        {
            for (int i = _activeBuffs.Count - 1; i >= 0; i--)
            {
                _activeBuffs[i].Duration -= deltaTime;
                if (_activeBuffs[i].Duration <= 0)
                {
                    _activeBuffs.RemoveAt(i);
                }
            }
        }

        private void MoveTo(Vector3 pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, Speed * Time.deltaTime);
            Vector3 direction = (pos - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                lookRotation.x = 0;
                lookRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        public bool TakeDamage(float damage, bool isSelf = false)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            blinkCoroutine = StartCoroutine(BlinkEffect());

            if (!isSelf)
            {
                var damageText = poolManager.Pop(damageTextItem) as DamageText;
                damageText?.SetDamageText(transform.position, damage);
            }

            _currentHP -= damage;

            if (_isBoss && _bossHPModel != null)
            {
                _bossHPModel.SetBossHP(_currentHP);
            }

            if (_currentHP <= 0)
            {
                _currentHP = 0;
                _resourceModel.AddGold(_enemyData.DropGold);
                OnDead();
                return true; // 적이 죽음
            }
            return false; // 적이 죽지 않음
        }

        private IEnumerator BlinkEffect()
        {
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float flashAmount = Mathf.Lerp(0f, 1f, elapsedTime / (flashDuration / 2));
                foreach (var m in mats)
                {
                    m.SetFloat("_BlinkAmount", flashAmount);
                }
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < flashDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float flashAmount = Mathf.Lerp(1f, 0f, elapsedTime / (flashDuration / 2));
                foreach (var m in mats)
                {
                    m.SetFloat("_BlinkAmount", flashAmount);
                }
                yield return null;
            }

            foreach (var m in mats)
            {
                m.SetFloat("_BlinkAmount", 0f);
            }
            blinkCoroutine = null;
        }

        public override void OnDead()
        {
            base.OnDead();
            OnEnemyDeadEvent?.Invoke(this);
            EffectPlayer player = poolManager.Pop(deadEffect) as EffectPlayer;
            player.PlayEffect(transform.position);
        }

        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
        }

        private void OnEnable()
        {
            ResetStats();
            _currentHP = Health;
            _activeBuffs.Clear();
            IsDead = false;
            if (_isBoss)
            {
                _bossHPModel.BossName = _enemyData.EnemyName;

                _bossHPModel.SetBossHP(0);
                _bossHPModel.SetBossHP(Health);
            }
        }
    }
}