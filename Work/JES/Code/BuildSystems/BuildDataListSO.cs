using System.Collections.Generic;
using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    [CreateAssetMenu(fileName = "BuildDataListSO", menuName = "SO/Build/DataList", order = 0)]
    public class BuildDataListSO : ScriptableObject
    {
        [SerializeField] private List<BuildDataSO> buildDataList;
        
        public BuildDataSO GetRandomBuildData()=> buildDataList[Random.Range(0, buildDataList.Count)];
    }
}