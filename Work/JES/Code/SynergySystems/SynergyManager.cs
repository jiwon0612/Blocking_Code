using System.Collections.Generic;
using UnityEngine;
using Work.JES.Code.BuildSystems;

namespace Work.JES.Code.SynergySystems
{
    public class SynergyManager : MonoBehaviour
    {
        [SerializeField] private TowerSynergy synergyPrefab;
        [SerializeField] private MapManageSO mapManageSO;

        private List<TowerSynergy> _synergyList = new List<TowerSynergy>();
        public void HandleBuildEnd(GameObject tower)
        {
            var lists = mapManageSO.GetSynergyTowers(tower);
            
            if (lists.Count > 1)
            {
                foreach (var data in lists)
                {
                    if (data.tower.synergyObj != null)
                    {
                        foreach (var item in lists)
                        {
                            if(item.tower.synergyObj!=null&& item.tower.synergyObj != data.tower.synergyObj)
                            {
                                item.tower.synergyObj.RemoveTower(item.tower);
                            }
                            data.tower.synergyObj.AddTower(item);
                        }
                        return;
                    }
                }
                TowerSynergy synergy = Instantiate(synergyPrefab);
                _synergyList.Add(synergy);
                synergy.Init(lists);  
            }
        }
    }
}