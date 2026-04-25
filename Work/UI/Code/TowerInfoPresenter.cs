using R3;
using UnityEngine;

namespace UI
{
    public class TowerInfoPresenter : MonoBehaviour
    {
        [SerializeField] private TowerInfoView view;
        [SerializeField] private TowerInfoModelSO modelSO;
        private void Start()
        {
            modelSO.TowerInfo
                .Subscribe(towerInfo => view.ShowInfo(towerInfo))
                .AddTo(this);
        }
    }
}