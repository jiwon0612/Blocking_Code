using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Work.JES.Code.Systems;
using Core.Dependencies;

namespace Work.CJW.Code.UI
{
    public class StageSelectUI : MonoBehaviour
    {

        public List<StageData> stageList;
        [SerializeField] private float widthValue;
        [SerializeField] private RectTransform stagePannel;
        [SerializeField] private StageImage stageSelectImagePrefab;
        [SerializeField] private float animationDuration;
        [SerializeField] private CanvasGroup next, prev;

        [Inject] private SceneLoader _sceneLoader;

        private int _currentIndex;
        private Tween _animationTween;

        private void Awake()
        {
            stagePannel.sizeDelta += new Vector2(stageList.Count - 1 * widthValue, 0f);
            for (int i = 0; i < stageList.Count; i++)
            {
                StageImage stageSelectImage = Instantiate(stageSelectImagePrefab, stagePannel);
                stageSelectImage.Init(stageList[i].stageScene, stageList[i].stageName);
                stageSelectImage.GetComponent<Image>().sprite = stageList[i].stageImage;

                RectTransform retTrm = stageSelectImage.GetComponent<RectTransform>();

                retTrm.localPosition = new Vector3(widthValue / 2 + (widthValue * i), 0, 0);
            }
            prev.alpha = 0f;
            prev.interactable = false;
        }

        public void ClickNext()
        {
            if (_currentIndex >= stageList.Count - 1) return;

            ButtonFade(prev, false);
            ButtonFade(next, false);

            _currentIndex++;

            if (_animationTween.IsActive())
                _animationTween.Complete();
            _animationTween = stagePannel.DOLocalMoveX(stagePannel.localPosition.x - widthValue, animationDuration)
                .OnComplete(() =>
                {
                    ButtonFade(prev, _currentIndex > 0);
                    ButtonFade(next, _currentIndex < stageList.Count - 1);
                });
        }

        public void ClickBack()
        {
            if (_currentIndex <= 0) return;

            _currentIndex--;

            ButtonFade(prev, false);
            ButtonFade(next, false);

            if (_animationTween.IsActive())
                _animationTween.Complete();
            _animationTween = stagePannel.DOLocalMoveX(stagePannel.localPosition.x + widthValue, animationDuration)
                .OnComplete(() =>
                {
                    ButtonFade(prev, _currentIndex > 0);
                    ButtonFade(next, _currentIndex < stageList.Count - 1);
                });
        }

        public void GoTitle()
        {
            if (_sceneLoader == null)
            {
                _sceneLoader = FindAnyObjectByType<SceneLoader>();
            }
            _sceneLoader.LoadScene("Title");
        }

        private void ButtonFade(CanvasGroup group, bool isActive)
        {
            if (isActive)
            {
                group.DOFade(1f, animationDuration).SetUpdate(true);
                group.interactable = true;
            }
            else
            {
                group.DOFade(0f, animationDuration).SetUpdate(true);
                group.interactable = false;
            }
        }
    }
}