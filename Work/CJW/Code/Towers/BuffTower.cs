using System.Collections.Generic;
using UnityEngine;
using Work.JES.Code.BuildSystems;

namespace Towers
{
    public class BuffTower : Tower
    {
        [SerializeField] private MapManageSO mapManager;
        [SerializeField] private float buffValue;

        public override void Init()
        {
            base.Init();
            SetBuff();
        }

        private void SetBuff()
        {
            List<TowerPlaceData> dataList = mapManager.GetAdjacentTowers(gameObject);
            foreach (var tower in dataList)
            {
                if (tower.towerObj.TryGetComponent(out Tower towerObj))
                {
                    towerObj.SetAttackDelayValue(buffValue);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            SetBuff();
        }

        public override void DestroyTower()
        {
            base.DestroyTower();
            List<TowerPlaceData> dataList = mapManager.GetAdjacentTowers(gameObject);
            foreach (var tower in dataList)
            {
                if (tower.towerObj.TryGetComponent(out Tower towerObj))
                {
                    towerObj.SetAttackDelayValue(1);
                }
            }
        }

        public override void Shooting(Transform target)
        {
            
        }
    }
}