using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Core.Dependencies;
using Core.ObjectPool.Runtime;
using Towers;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Work.JES.Code.BuildSystems
{
    public class BuildManager : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private MapManageSO mapManageSO;
        [SerializeField] private InputSO inputSO;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private BuildDataListSO dataList;
        [SerializeField] private PreviewTower previewPrefab;
        [SerializeField] private TowerInfoModelSO towerInfoModel;
        [SerializeField] private BuyButtonModelSO buyButtonModel;
        [SerializeField] private TileBuildManager tileBuildManager;

        private BuildDataSO _currentData;
        private PreviewTower _previewTower;

        private bool _isBuilding = false;
        private bool _isCanBuild = false;
        private int _rotationIndex = 0;

        public UnityEvent<GameObject> BuildEndEvent;
        public UnityEvent ThrowAwayTowerEvent;
        private void Awake()
        {
            var tileList = GetComponentsInChildren<BuildTile>().ToList();

            mapManageSO.InitTile(tileList);

            inputSO.OnClickEvent += HandleClickEvent;
            inputSO.OnRotateEvent += HandleRotateEvent;
            inputSO.OnRightClickEvent += HandleRightClickEvent;
        }

        private void OnDestroy()
        {
            inputSO.OnClickEvent -= HandleClickEvent;
            inputSO.OnRotateEvent -= HandleRotateEvent;
            inputSO.OnRightClickEvent -= HandleRightClickEvent;
        }
        private void HandleRightClickEvent()
        {
            if (_isBuilding)
            {
                EndBuild(false);
            }
        }

        private void HandleRotateEvent(int direction)
        {
            if (_isBuilding == false) return;
            _rotationIndex += direction;
            if (_rotationIndex < 0)
                _rotationIndex = 3;
            else if (_rotationIndex > 3)
                _rotationIndex = 0;
            int rotationAngle = direction==1 ? 90 : -90;
            _previewTower.transform.DOComplete();
            _previewTower.transform.DORotate(_previewTower.transform.eulerAngles+ new Vector3(0, rotationAngle, 0), 0.2f);
        }

        private void HandleClickEvent()
        {
            if (_isBuilding && _isCanBuild)
            {
                _isBuilding = false;
                TowerPlaceData towerPlaceData = new TowerPlaceData();

                Vector3Int centerPosition = _previewTower.transform.position.ToVector3Int();
                Tower tower = _poolManager.Pop<Tower>(_currentData.towerPrefab.PoolingType);
                towerPlaceData.towerObj = tower.gameObject;
                towerPlaceData.tower = tower;
                tower.transform.DOScale(Vector3.one, 0.4f).OnComplete(() => tower.Init());

                tower.transform.position = centerPosition;
                tower.transform.GetChild(0).rotation = _previewTower.transform.rotation;


                towerPlaceData.positions = new List<Vector3Int>();
                mapManageSO.GetTile(centerPosition).BuildingTile();
                towerPlaceData.positions.Add(centerPosition);
                if(!_currentData.IsOne)
                {
                    foreach (Vector3Int position in _currentData.GetTowerTilePositions(_rotationIndex))
                    {
                        Vector3Int towerPosition = position + centerPosition;
                        mapManageSO.GetTile(towerPosition).BuildingTile();
                        towerPlaceData.positions.Add(towerPosition);
                    }
                }
                mapManageSO.PlaceTower(towerPlaceData);
                EndBuild();
                BuildEndEvent?.Invoke(tower.gameObject);
            }
        }

        private void Update()
        {
            if (_isBuilding)
            {
                if (inputSO.IsGround)
                {
                    _isCanBuild = mapManageSO.CanPlaceTower(inputSO.MousePosition, _currentData.GetTowerTilePositions(_rotationIndex));

                    if (_previewTower != null)
                    {
                        _previewTower.transform.position = inputSO.MousePosition;
                        _previewTower.SetMatColor(_isCanBuild);
                    }
                }
            }
        }

        private void EndBuild(bool isInstalled = true)
        {
            if(!isInstalled)
                ThrowAwayTowerEvent?.Invoke();
            
            _isBuilding = false;
            mapManageSO.ResetTile();
            Destroy(_previewTower.gameObject);
            towerInfoModel.TowerInfo.Value = null;
            buyButtonModel.IsEnabled.Value = true;
        }

        public void BuildStart()
        {
            _currentData = dataList.GetRandomBuildData();
            if (_currentData.towerName == "타일")
            {
                tileBuildManager.StartBuilding();
                return;
            }
            _rotationIndex = 0;
            _isCanBuild = false;
            _isBuilding = true;
            _previewTower = Instantiate(previewPrefab);
            _previewTower.InitMesh(_currentData.towerMesh,_currentData.towerPrefab.AttackDistance);
            mapManageSO.ResetTile();

            towerInfoModel.TowerInfo.Value = _currentData;

            buyButtonModel.IsEnabled.Value = false;
        }
    }
}