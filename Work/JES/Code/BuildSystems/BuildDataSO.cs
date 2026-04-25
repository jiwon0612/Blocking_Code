using System;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "SO/Build/Data", order = 0)]
    public class BuildDataSO : ScriptableObject
    {
        [SerializeField] private List<Vector3Int> towerTilePositions;
        public string towerName;
        [TextArea]
        public string towerDescription;

        public bool IsOne => towerTilePositions.Count <= 0;
        public Mesh towerMesh => towerPrefab.MeshFilter.sharedMesh;
        public Tower towerPrefab;
        [HideInInspector] public List<List<Vector3Int>> towerTilePositionLists; // 0도, 90도, 180도, 270도

        private void OnEnable()
        {
            GenerateRotatedTileLists();
        }
        public List<Vector3Int> GetTowerTilePositions(int rotationIndex)
        {
            if (rotationIndex < 0 || rotationIndex >= towerTilePositionLists.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(rotationIndex), "Rotation index must be between 0 and 3.");
            }
            return towerTilePositionLists[rotationIndex];
        }

        private void GenerateRotatedTileLists()
        {
            towerTilePositionLists = new List<List<Vector3Int>>();

            // 0도 (원본)
            towerTilePositionLists.Add(new List<Vector3Int>(towerTilePositions));

            // 90도, 180도, 270도 회전 좌표 생성
            for (int i = 1; i < 4; i++)
            {
                towerTilePositionLists.Add(Rotate90(towerTilePositionLists[i - 1]));
            }
        }

        private List<Vector3Int> Rotate90(List<Vector3Int> input)
        {
            List<Vector3Int> result = new List<Vector3Int>();

            foreach (var pos in input)
            {
                // 기준점 (0, 0, 0) 기준 Y축 회전 (90도)
                int x = pos.x;
                int z = pos.z;

                int newX = z;
                int newZ = -x;

                result.Add(new Vector3Int(newX, pos.y, newZ));
            }

            return result;
        }
    }
}