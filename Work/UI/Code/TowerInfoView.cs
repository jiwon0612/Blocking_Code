using R3;
using System;
using TMPro;
using UnityEngine;
using Work.JES.Code.BuildSystems;
using Work.JES.Code.SynergySystems;

namespace UI
{
    public class TowerInfoView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text towerNameText;
        [SerializeField] private TMP_Text towerDescriptionText;
        [SerializeField] private TMP_Text type;
 
        private IDisposable _fadeAnimationDisposable;
        private float _currentAlpha = 0f;

        /// <summary>
        /// 페이드 인 애니메이션임. null들어오면 페이드 아웃 애니메이션 실행
        /// </summary>
        /// <param name="buildData"></param>
        /// <param name="fadeDuration"></param>
        public void ShowInfo(BuildDataSO buildData, float fadeDuration = 0.3f)
        {
            if (buildData == null)
            {
                HideInfo(fadeDuration);
                return;
            }

            _currentAlpha = 0f;
            SetTowerInfoText(buildData.towerName, buildData.towerDescription, buildData.towerPrefab.SynergyType);

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
        private void HideInfo(float fadeDuration = 0.3f)
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

        private void SetTowerInfoText(string towerName, string towerDescription)
        {
            towerNameText.text = towerName;
            towerDescriptionText.text = towerDescription;
        }
        private void SetTowerInfoText(string towerName, string towerDescription, SunergyType type)
        {
            towerNameText.text = towerName;
            towerDescriptionText.text = towerDescription;
            this.type.text = type.ToString();
        }
    }
}