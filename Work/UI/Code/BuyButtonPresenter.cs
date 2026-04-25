using R3;
using UnityEngine;

namespace UI
{
    public class BuyButtonPresenter : MonoBehaviour
    {
        [SerializeField] private BuyButtonView view;
        [SerializeField] private BuyButtonModelSO modelSO;
        [SerializeField] private ResourceModelSO resourceModelSO;

        private void Start()
        {
            view.OnBuyButtonClick
                .Subscribe(_ => OnBuyButtonClick())
                .AddTo(this);

            modelSO.IsEnabled
                .Subscribe(UpdateButtonState)
                .AddTo(this);

            resourceModelSO.Gold
                .Subscribe(_ => UpdateButtonState(modelSO.IsEnabled.Value))
                .AddTo(this);
        }

        private void UpdateButtonState(bool isEnabled)
        {
            if (!isEnabled)
            {
                view.SetButtonInteractable(false);
                view.SetButtonText(modelSO.buildingText);
                return;
            }

            if (CanBuy())
            {
                view.SetButtonInteractable(true);
                view.SetButtonText(modelSO.BuyText);
            }
            else
            {
                view.SetButtonInteractable(false);
                view.SetButtonText(modelSO.insufficientFundsText);
            }
        }

        private void OnBuyButtonClick()
        {
            if (CanBuy())
            {
                resourceModelSO.SubtractGold(modelSO.Cost);
                Debug.Log("구매 성공");
            }
        }

        private bool CanBuy() => resourceModelSO.Gold.Value >= modelSO.Cost;
    }
}
