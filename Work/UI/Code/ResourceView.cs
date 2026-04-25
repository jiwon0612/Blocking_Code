using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using R3;

namespace UI
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private Transform heartParent;
        [SerializeField] private GameObject heartIconPrefab;
        [SerializeField] private Sprite fillHeart, unFillHeart;

        [SerializeField] private TMP_Text goldText;

        private int _currentDisplayGold = 0;
        private IDisposable _goldAnimationDisposable;
        private List<Image> _heartIcons = new();

        public void InitializeHealthUI(int maxHealth)
        {
            foreach (Transform child in heartParent)
            {
                Destroy(child.gameObject);
            }
            _heartIcons.Clear();

            for (int i = 0; i < maxHealth; i++)
            {
                GameObject heartObj = Instantiate(heartIconPrefab, heartParent);
                _heartIcons.Add(heartObj.GetComponent<Image>());
            }
        }

        public void UpdateHealth(int currentHealth)
        {
            for (int i = 0; i < _heartIcons.Count; i++)
            {
                _heartIcons[i].sprite = i < currentHealth ? fillHeart : unFillHeart;
            }
        }

        private void SetGoldText(int currentGold)
        {
            goldText.text = currentGold.ToString("N0");
            _currentDisplayGold = currentGold;
        }

        public void AnimateGoldTo(int targetGold, float duration = 0.5f)
        {
            if (duration <= 0f)
            {
                SetGoldText(targetGold);
                return;
            }

            int startGold = _currentDisplayGold;
            if (startGold == targetGold)
            {
                SetGoldText(targetGold);
                return;
            }

            _currentDisplayGold = goldText.text.Length > 0
                ? int.Parse(goldText.text.Replace(",", ""))
                : 0;

            _goldAnimationDisposable?.Dispose();

            _goldAnimationDisposable = Observable.EveryUpdate()
                .Scan(0f, (elapsedTime, _) => elapsedTime + Time.deltaTime)
                .TakeWhile(elapsedTime => elapsedTime <= duration)
                .Subscribe(elapsedTime =>
                {
                    float progress = Mathf.Clamp01(elapsedTime / duration);
                    int animatedGold = (int)Mathf.Lerp(startGold, targetGold, progress);
                    SetGoldText(animatedGold);
                },
                elapsedTime => SetGoldText(targetGold));
        }


        private void OnDestroy()
        {
            _goldAnimationDisposable?.Dispose();
        }
    }
}