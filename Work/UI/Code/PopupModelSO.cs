using R3;
using System;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "PopupModel", menuName = "SO/UIModels/PopupModel")]
    public class PopupModelSO : ScriptableObject, IDisposable
    {
        // 라벨 스프라이트
        [SerializeField] private Sprite blueLabel;
        [SerializeField] private Sprite redLabel;
        [SerializeField] private Sprite orangeLabel;
        [SerializeField] private Sprite purpleLabel;
        // 버튼 스프라이트
        [SerializeField] private Sprite blueButton;
        [SerializeField] private Sprite grayButton;
        [SerializeField] private Sprite greenButton;
        [SerializeField] private Sprite mintButton;
        [SerializeField] private Sprite navyButton;
        [SerializeField] private Sprite orangeButton;
        [SerializeField] private Sprite pinkButton;
        [SerializeField] private Sprite purpleButton;
        [SerializeField] private Sprite redButton;
        [SerializeField] private Sprite yellowButton;


        public ReactiveProperty<PopupData> PopupData { get; private set; } = new();

        public void SetPopup(string title, string message, string buttonText, LabelColor labelColor, ButtonColor buttonColor, Action onButtonClick, bool isNew = false)
        {
            PopupData.Value = new PopupData(
                title,
                message,
                buttonText,
                GetLabelSprite(labelColor),
                GetButtonSprite(buttonColor),
                onButtonClick,
                isNew
            );
        }

        private Sprite GetLabelSprite(LabelColor color)
        {
            return color switch
            {
                LabelColor.Blue => blueLabel,
                LabelColor.Red => redLabel,
                LabelColor.Orange => orangeLabel,
                LabelColor.Purple => purpleLabel,
                _ => null
            };
        }
        private Sprite GetButtonSprite(ButtonColor color)
        {
            return color switch
            {
                ButtonColor.Blue => blueButton,
                ButtonColor.Gray => grayButton,
                ButtonColor.Green => greenButton,
                ButtonColor.Mint => mintButton,
                ButtonColor.Navy => navyButton,
                ButtonColor.Orange => orangeButton,
                ButtonColor.Pink => pinkButton,
                ButtonColor.Purple => purpleButton,
                ButtonColor.Red => redButton,
                ButtonColor.Yellow => yellowButton,
                _ => null
            };
        }

        public void Dispose()
        {
            PopupData?.Dispose();
        }
    }
}