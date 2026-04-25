using R3;
using System;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "ResourceModel", menuName = "SO/UIModels/ResourceModel")]
    public class ResourceModelSO : ScriptableObject, IDisposable
    {
        [SerializeField] private int initialHealth = 3;
        [SerializeField] private int initialGold = 0;

        // 런타임에서만 사용할 변수
        public ReactiveProperty<int> Gold { get; private set; }
        public ReactiveProperty<int> Health { get; private set; }

        private int _maxGold = 100000000;
        private int _maxHealth = 10;

        private void OnEnable()
        {
            ResetProperty();
        }

        public void ResetProperty()
        {
            Gold = new ReactiveProperty<int>(initialGold);
            Health = new ReactiveProperty<int>(initialHealth);
        }

        public void AddGold(int amount)
        {
            Gold.Value = Mathf.Clamp(Gold.Value + amount, 0, _maxGold);
        }
        public void AddHealth(int amount)
        {
            Health.Value = Mathf.Clamp(Health.Value + amount, 0, _maxHealth);
        }
        public void SubtractGold(int amount)
        {
            Gold.Value = Mathf.Clamp(Gold.Value - amount, 0, _maxGold);
        }
        public void SubtractHealth(int amount)
        {
            Health.Value = Mathf.Clamp(Health.Value - amount, 0, _maxHealth);
        }
        public void Dispose()
        {
            Gold?.Dispose();
            Health?.Dispose();
        }
    }
}