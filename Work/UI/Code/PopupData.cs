using System;
using UnityEngine;

namespace UI
{
    public enum LabelColor
    {
        Blue,
        Red,
        Orange,
        Purple
    }
    public enum ButtonColor
    {
        Blue,
        Gray,
        Green,
        Mint,
        Navy,
        Orange,
        Pink,
        Purple,
        Red,
        Yellow
    }
    public struct PopupData
    {
        public string Title;
        public string Message;
        public string ButtonText;
        public Sprite LabelSprite;
        public Sprite ButtonSprite;
        // 버튼에 할당할 메소드
        public Action OnButtonClick;
        public bool IsNewScore;

        public PopupData(string title, string message, string buttonText, Sprite label, Sprite button, Action onButtonClick, bool isNew = false)
        {
            Title = title;
            Message = message;
            ButtonText = buttonText;
            LabelSprite = label;
            ButtonSprite = button;
            OnButtonClick = onButtonClick;
            IsNewScore = isNew;
        }
    }
}
