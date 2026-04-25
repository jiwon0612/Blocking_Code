using R3;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BossHPView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text bossName;
        [SerializeField] private Slider hpSlider;

        private IDisposable _fadeAnimationDisposable;
        private float _currentAlpha = 0f;

        public void SetBossName(string name)
        {
            bossName.text = name;
        }
        public void SetBossHP(float currentHP, float maxHP)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        public void Show(float fadeDuration = 0.3f)
        {
            _currentAlpha = 0f;
            _fadeAnimationDisposable?.Dispose();
            _fadeAnimationDisposable = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _currentAlpha = Mathf.MoveTowards(_currentAlpha, 1f, Time.deltaTime / fadeDuration);
                    canvasGroup.alpha = _currentAlpha;
                    if (_currentAlpha >= 1f)
                    {
                        _fadeAnimationDisposable?.Dispose();
                    }
                });
        }
        public void Fade(float fadeDuration = 0.3f)
        {
            _currentAlpha = 1f;
            _fadeAnimationDisposable?.Dispose();
            _fadeAnimationDisposable = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _currentAlpha = Mathf.MoveTowards(_currentAlpha, 0f, Time.deltaTime / fadeDuration);
                    canvasGroup.alpha = _currentAlpha;
                    if (_currentAlpha <= 0f)
                    {
                        _fadeAnimationDisposable?.Dispose();
                    }
                });
        }
    }
}