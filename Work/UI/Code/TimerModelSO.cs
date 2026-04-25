using R3;
using System;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "TimerModel", menuName = "SO/UIModels/TimerModelSO")]
    public class TimerModelSO : ScriptableObject, IDisposable
    {
        public ReactiveProperty<float> Timer = new();
        public void Dispose()
        {
            Timer?.Dispose();
        }

        internal void ResetProperty()
        {
            Timer.Value = 0f;
        }
    }
}