using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup, newScoreImage;

        [SerializeField] private Image labelImage, buttonImage;
        [SerializeField] private TMP_Text titleText, messageText, buttonText;
        [SerializeField] private Button button;

        public void SetPopup(PopupData popupData)
        {
            if (popupData.Title == null)
            {
                HidePopup();
                return;
            }
            titleText.text = popupData.Title;
            messageText.text = popupData.Message;
            this.buttonText.text = popupData.ButtonText;
            labelImage.sprite = popupData.LabelSprite;
            buttonImage.sprite = popupData.ButtonSprite;

            button.onClick.AddListener(() =>
            {
                HidePopup();
                popupData.OnButtonClick?.Invoke();
            });

            newScoreImage.alpha = popupData.IsNewScore ? 1f : 0f;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        public void HidePopup()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            button.onClick.RemoveAllListeners();
        }
    }
}
