using R3;
using UnityEngine;

namespace UI
{
    public class ResourcePresenter : MonoBehaviour
    {
        [SerializeField] private ResourceView view;

        [SerializeField] private ResourceModelSO modelSO;

        private void Start()
        {
            view.InitializeHealthUI(modelSO.Health.Value);
            view.AnimateGoldTo(modelSO.Gold.Value, 0f);

            modelSO.Health
                .Subscribe(health => view.UpdateHealth(health))
                .AddTo(this);
            modelSO.Gold
                .Subscribe(goldAmount => view.AnimateGoldTo(goldAmount))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            modelSO?.Dispose();
        }
    }
}