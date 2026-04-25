using System;
using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    public class TileBuildManager : MonoBehaviour
    {
        [SerializeField] private MapManageSO mapManageSO;
        [SerializeField] private InputSO inputSO;
        [SerializeField] private PreviewTower previewPrefab;
        [SerializeField] private BuildTile tilePrefab;
        
        private PreviewTower _previewTower;
        private bool _isBuilding = false;
        private bool _canBuild = false;

        private void Awake()
        {
            inputSO.OnClickEvent += HandleClickEvent;
            inputSO.OnRightClickEvent += HandleRightClickEvent;
        }

        private void OnDestroy()
        {
            inputSO.OnClickEvent -= HandleClickEvent;
            inputSO.OnRightClickEvent -= HandleRightClickEvent;
        }

        private void Update()
        {
            if (_isBuilding)
            {
                if (inputSO.IsGround)
                {
                   _canBuild= mapManageSO.CanBuildTile(inputSO.MousePosition);

                    if (_previewTower != null)
                    {
                        _previewTower.transform.position = inputSO.MousePosition+ new Vector3(0, 1, 0);
                        _previewTower.SetMatColor(_canBuild);
                    }
                }
            }
        }
        private void HandleRightClickEvent()
        {
            if (_isBuilding)
            {
                _isBuilding = false;
                if (_previewTower != null)
                {
                    Destroy(_previewTower.gameObject);
                }
                _canBuild = false;
            }
        }

        private void HandleClickEvent()
        {
            if (_isBuilding && _canBuild)
            {
                _isBuilding = false;
                
                TowerPlaceData towerPlaceData = new TowerPlaceData();
                
                Vector3Int centerPosition = _previewTower.transform.position.ToVector3Int();
                centerPosition.y = 0;
                BuildTile trm = Instantiate(tilePrefab);
                
                mapManageSO.BuildGroundTile(trm,centerPosition);
                trm.transform.position = centerPosition;
                
                mapManageSO.ResetTile();
                Destroy(_previewTower.gameObject);
            }
        }
        public void StartBuilding()
        {
            if (!_isBuilding)
            {
                
                _isBuilding = true;
                _canBuild = false;
                mapManageSO.ResetTile();
                
                if (_previewTower == null)
                {
                    _previewTower = Instantiate(previewPrefab);
                    _previewTower.InitMesh(tilePrefab.GetComponent<MeshFilter>().sharedMesh,0);
                }
            }
        }
    }
}