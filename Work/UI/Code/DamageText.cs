using Core.ObjectPool.Runtime;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DamageText : MonoBehaviour, IPoolable 
    {
        [SerializeField] private TextMeshPro damageText;
        [SerializeField] private float duration = 1.5f;
        [SerializeField] private float moveRange = 2f;
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;

        private Pool _pool;
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            damageText.alpha = 1;
        }
        
        public void SetDamageText(Vector3 spawnPos, float damage)
        {
            Sequence seq = DOTween.Sequence();
            spawnPos.y += 1f;
            transform.position = spawnPos + Random.insideUnitSphere * 0.3f;
            damageText.text = $"{damage}";
            seq.Append(transform.DOMoveY(transform.position.y + moveRange, duration))
                .Join(damageText.DOFade(0, duration)).OnComplete(() => _pool.Push(this));
        }

        
    }
}