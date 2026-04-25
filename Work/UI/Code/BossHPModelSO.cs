using R3;
using System;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "BossHPModel", menuName = "SO/UIModels/BossHPModel")]
    public class BossHPModelSO : ScriptableObject, IDisposable
    {
        public ReactiveProperty<float> BossHP { get; private set; } = new(0f);
        public string BossName;

        public void SetBossHP(float hp)
        {
            BossHP.Value = Mathf.Max(0, hp);
        }
        public void Dispose()
        {
            BossHP?.Dispose();
        }
    }
}