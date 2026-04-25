using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    public class BuildTile : MonoBehaviour
    {
        public bool _canBuild = true;

        public void BuildingTile()
        {
            _canBuild = false;
            gameObject.SetActive(false);
        }
        public void TurnOnTile()
        {
            if(_canBuild)
                gameObject.SetActive(true);
        }
    }
}