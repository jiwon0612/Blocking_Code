using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    public struct TowerPlaceData
    {
        public Tower tower;
        public List<Vector3Int> positions;
        public GameObject towerObj;
    }

    [CreateAssetMenu(menuName = "SO/Map/Manager")]
    public class MapManageSO : ScriptableObject
    {
        private List<Vector3Int> _towerTilePositionList = new List<Vector3Int>();
        private List<Vector3Int> _groundTilePositionList = new List<Vector3Int>();
        private Dictionary<Vector3Int, TowerPlaceData> _towerMapDic = new Dictionary<Vector3Int, TowerPlaceData>();
        private Dictionary<Vector3Int, BuildTile> _tileMapDic = new Dictionary<Vector3Int, BuildTile>();
        public void InitTile(List<BuildTile> tiles)
        {
            _groundTilePositionList.Clear();
            _towerTilePositionList.Clear();
            _towerMapDic.Clear();
            _tileMapDic.Clear();

            foreach (var tile in tiles)
            {
                Vector3Int position = tile.transform.localPosition.ToVector3Int();
                position.y = 0;
                _tileMapDic.Add(position, tile);
                _groundTilePositionList.Add(position);
            }
            ResetTile();
        }

        public BuildTile GetTile(Vector3Int position)
        {
            if (_tileMapDic.TryGetValue(position, out BuildTile tile))
            {
                return tile;
            }
            return null;
        }


        public void ResetTile()
        {
            _tileMapDic.Values.ToList().ForEach(tile => tile.gameObject.SetActive(false));
            _tileMapDic.Values.ToList().ForEach(tile => tile.TurnOnTile());
        }
        public bool CanBuildTile(Vector3Int position)
        {
            return (!_groundTilePositionList.Contains(position) && !_towerTilePositionList.Contains(position));
        }

        public bool CanPlaceTower(Vector3Int position, List<Vector3Int> positions)
        {
            if (_towerTilePositionList.Contains(position) || !_groundTilePositionList.Contains(position))
            {
                return false;
            }
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3Int pos = positions[i] + position;
                if (_towerTilePositionList.Contains(pos) || !_groundTilePositionList.Contains(pos))
                {
                    return false;
                }
            }
            return true;
        }

        public void BuildGroundTile(BuildTile tile, Vector3Int position)
        {
            if (!_groundTilePositionList.Contains(position))
            {
                _tileMapDic.Add(position, tile);
                _groundTilePositionList.Add(position);
            }
        }

        public void PlaceTower(TowerPlaceData towerPlaceData)
        {
            foreach (Vector3Int position in towerPlaceData.positions)
            {
                _towerMapDic.Add(position, towerPlaceData);
                if (!_towerTilePositionList.Contains(position))
                {
                    _towerTilePositionList.Add(position);
                }
            }
        }

        public void RemoveTower(Vector3Int position)
        {
            if (_towerMapDic.ContainsKey(position))
            {
                TowerPlaceData towerPlaceData = _towerMapDic[position];
                _towerMapDic.Remove(position);
                foreach (Vector3Int pos in towerPlaceData.positions)
                {
                    _towerMapDic.Remove(pos);
                    _tileMapDic[pos]._canBuild = true;
                    _towerTilePositionList.Remove(pos);
                }

                _tileMapDic[position].BuildingTile();
                _groundTilePositionList.Remove(position);
                ResetTile();
                towerPlaceData.tower.DestroyTower();
            }
        }

        public Vector3Int CloseTowerPositionFromEnemyPosition(Vector3 enemyPosition)
        {
            if (_towerTilePositionList.Count <= 0) return Vector3Int.zero;
            int closeIndex = 0;
            float minDinstance = Vector3.Distance(_towerTilePositionList[0], enemyPosition);
            for (int i = 0; i < _towerTilePositionList.Count; i++)
            {
                float distance = Vector3.Distance(_towerTilePositionList[i], enemyPosition);
                if (distance < minDinstance)
                {
                    closeIndex = i;
                    minDinstance = distance;
                }
            }

            return _towerTilePositionList[closeIndex];
        }

        private TowerPlaceData GeTowerPlaceData(GameObject towerObj)
        {
            if (towerObj == null) return new TowerPlaceData();
            return _towerMapDic.FirstOrDefault(x => x.Value.towerObj == towerObj).Value;
        }
        public List<TowerPlaceData> GetSynergyTowers(GameObject towerObj)
        {
            TowerPlaceData towerPlaceData = GeTowerPlaceData(towerObj);
            if (towerPlaceData.positions == null || towerPlaceData.positions.Count == 0)
                return new List<TowerPlaceData>();

            HashSet<TowerPlaceData> synergyTowers = new HashSet<TowerPlaceData>();
            Vector3Int[] directions =
            {
                Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back
            };
            foreach (var pos in towerPlaceData.positions)
            {
                foreach (var dir in directions)
                {
                    Vector3Int neighborPos = pos + dir;
                    if (_towerMapDic.TryGetValue(neighborPos, out var neighborTower))
                    {
                        if (neighborTower.tower.SynergyType == towerPlaceData.tower.SynergyType)
                        {
                            synergyTowers.Add(towerPlaceData);
                            synergyTowers.Add(neighborTower);
                        }
                    }
                }
            }

            return synergyTowers.ToList();
        }
        public List<TowerPlaceData> GetAdjacentTowers(GameObject towerObj)
        {
           TowerPlaceData towerPlaceData = GeTowerPlaceData(towerObj);
            if (towerPlaceData.positions == null || towerPlaceData.positions.Count == 0)
                return new List<TowerPlaceData>();

            HashSet<TowerPlaceData> adjacentTowers = new HashSet<TowerPlaceData>();
            Vector3Int[] directions =
            {
                Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back
            };

            foreach (var pos in towerPlaceData.positions)
            {
                foreach (var dir in directions)
                {
                    Vector3Int neighborPos = pos + dir;
                    if (_towerMapDic.TryGetValue(neighborPos, out var neighborTower))
                    {
                        if (!neighborTower.Equals(towerPlaceData))
                            adjacentTowers.Add(neighborTower);
                    }
                }
            }

            return adjacentTowers.ToList();
        }
    }
}