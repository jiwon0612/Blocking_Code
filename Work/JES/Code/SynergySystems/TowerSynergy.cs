using System.Collections.Generic;
using EPOOutline;
using Towers;
using UnityEngine;
using Work.CJW.Code.SynergySkills;
using Work.JES.Code.BuildSystems;

namespace Work.JES.Code.SynergySystems
{
    public enum SunergyType
    {
        Red,Blue,Green
    }
    public class TowerSynergy : MonoBehaviour
    {
        [ColorUsage(true, true)] // 첫 번째 true: HDR 허용, 두 번째 true: 알파 허용]
        [SerializeField] private Color[] colors;
        private List<Tower> _towerList = new List<Tower>();
        public SunergyType SynergyType { get; private set; }
        [SerializeField] private float synergyDuration = 2f;
        private float _timer;
        private Outlinable _outline;
        private SynergyAction[] _synergyActions;
        private Dictionary<Tower,int> _towerTileCount = new Dictionary<Tower, int>();
        [SerializeField] private SynergyBtnUI _btnUI;

        public int tileCount;
        public void Init(List<TowerPlaceData> towerList)
        {
            SynergyType=towerList[0].tower.SynergyType;
            transform.position = towerList[0].tower.transform.position;
            _outline = GetComponent<Outlinable>();

            foreach (var tower in towerList)
            {
                AddTower(tower);
                _outline.AddRenderer(tower.tower.MeshFilter.GetComponent<MeshRenderer>());
            }
            _outline.OutlineParameters.Color = colors[(int)SynergyType];
            //_outline.AddAllChildRenderersToRenderingList();
            _outline.enabled = true;
            
            _synergyActions = GetComponentsInChildren<SynergyAction>();
            
            _btnUI.InitBtn(SynergyType,this);
        }
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= synergyDuration)
            {
                _timer = 0;
                _synergyActions[(int)SynergyType].ActiveSynergy();
            }
        }

        public void AddTower(TowerPlaceData tower)
        {
            if (_towerList.Contains(tower.tower)) return;
            
            tileCount += tower.positions.Count;
            
            tower.tower.synergyObj = this;
            _towerTileCount.Add(tower.tower,tower.positions.Count);
            _towerList.Add(tower.tower);
            tower.tower.transform.SetParent(transform);
            tower.tower.TowerDestroyedEvent.AddListener(HandleDeadTower);
            //_outline.AddAllChildRenderersToRenderingList();
            _outline.AddRenderer(tower.tower.MeshFilter.GetComponent<MeshRenderer>());
        }
        public void RemoveTower(Tower tower)
        {
            if (_towerList.Contains(tower) == false) return;
            HandleDeadTower(tower);
        }

        private void HandleDeadTower(Tower tower)
        {
            _towerList.Remove(tower);
            tileCount-= _towerTileCount[tower];
            _towerTileCount.Remove(tower);
            if (_towerList.Count <= 1)
            {
                foreach (var item in _towerList)
                {
                    item.synergyObj = null;
                    item.transform.parent = null;
                }
                tower.transform.parent = null;
                _synergyActions[(int)SynergyType].EndSynergy();
                Destroy(gameObject);
            }
        }
    }
}