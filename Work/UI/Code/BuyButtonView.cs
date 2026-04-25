using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuyButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text buttonText;

        public Observable<Unit> OnBuyButtonClick => button.OnClickAsObservable();

        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
        public void SetButtonInteractable(bool isInteractable)
        {
            button.interactable = isInteractable;
        }
    }
}
