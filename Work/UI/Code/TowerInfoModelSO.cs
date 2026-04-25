using R3;
using System;
using UnityEngine;
using Work.JES.Code.BuildSystems;

namespace UI
{
    [CreateAssetMenu(fileName = "TowerInfoModel", menuName = "SO/UIModels/TowerInfoModel")]
    public class TowerInfoModelSO : ScriptableObject, IDisposable
    {
        public ReactiveProperty<BuildDataSO> TowerInfo = new();
        private void OnEnable()
        {
            TowerInfo.Value = null;
        }

        public void Dispose()
        {
            TowerInfo?.Dispose();
        }
    }
}