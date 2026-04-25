using UnityEngine;
using System.Collections.Generic;
using Core.Dependencies;
using Core.ObjectPool.Runtime;
using System;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Inject]
        public PoolManagerMono poolManager;

        public Action OnSpawnEnd;

        [SerializeField] private List<PoolingItemSO> enemyItems;
        [SerializeField] private float spawnCountLimit = 10;// 0은 무제한
        [SerializeField] private float spawnRadius = 5f;
        [SerializeField] private int maxSpawnedEnemies = 10;
        [SerializeField] private float spawnHeight = 0.5f;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float spawnIntervalDecrease = 0.1f;
        [SerializeField] private float hpMultiplierIncrease = 0.1f;

        private List<Enemy> _activeEnemies = new();
        private float _spawnTimer;
        private float _spawnCount;

        private bool _isSpawnEnd = false;

        private void Awake()
        {
            _spawnTimer = spawnInterval;
        }

        private void FixedUpdate()
        {
            if (_isSpawnEnd) return;

            if (spawnCountLimit > 0 && _spawnCount >= spawnCountLimit)
            {
                if (_activeEnemies.Count == 0)
                {
                    OnSpawnEnd?.Invoke();
                    _isSpawnEnd = true;
                }
                return;
            }

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && _activeEnemies.Count < maxSpawnedEnemies)
            {
                _spawnCount++;
                SpawnEnemy();
                _spawnTimer = spawnInterval;
            }
        }
        

        private void SpawnEnemy()
        {
            Debug.Assert(enemyItems.Count > 0, "에너미 so넣어주세요.");

            var randomIndex = UnityEngine.Random.Range(0, enemyItems.Count);
            var item = enemyItems[randomIndex];

            var poolable = poolManager.Pop<IPoolable>(item);
            Debug.Assert(poolable != null, "풀에 없는데??");
            Debug.Assert(poolable != null, "풀에서 가져온 오브젝트가 IPoolable이 아닙니다.");
            Enemy enemy = poolable as Enemy;
            Debug.Assert(enemy != null, "풀에서 가져온 오브젝트가 Enemy가 아닙니다.");


            // 랜덤한 위치에 스폰
            Vector3 spawnPosition = transform.position + new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                spawnHeight,
                UnityEngine.Random.Range(-spawnRadius, spawnRadius)
            );
            enemy.transform.position = spawnPosition;
            _activeEnemies.Add(enemy);
            enemy.OnEnemyDeadEvent += OnEnemyDead; // 적이 죽었을 때 이벤트 등록

            enemy.HpMultiplier += hpMultiplierIncrease;
            spawnInterval = Mathf.Max(0.1f, spawnInterval - spawnIntervalDecrease);
        }

        private void OnEnemyDead(Enemy enemy)
        {
            if (_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Remove(enemy);
                poolManager.Push(enemy);
                enemy.OnEnemyDeadEvent -= OnEnemyDead;

                if (!_isSpawnEnd && spawnCountLimit > 0 && _spawnCount >= spawnCountLimit && _activeEnemies.Count == 0)
                {
                    OnSpawnEnd?.Invoke();
                    _isSpawnEnd = true;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}