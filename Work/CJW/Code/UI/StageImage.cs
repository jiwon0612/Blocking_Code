using Core.Dependencies;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Work.JES.Code.Systems;

namespace Work.CJW.Code.UI
{
    public class StageImage : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private float animationValue;
        [SerializeField] private float animationDuration;
        [SerializeField] private TMP_Text stageNameText;
        [Inject] private SceneLoader _sceneLoader;
        
        private Tween _animationTween;
        private string nextScene;
        
        public void Init(string nextScene, string stageName)
        {
            this.nextScene = nextScene;
            stageNameText.text = stageName;
            if (_sceneLoader == null)
            {
                _sceneLoader = FindAnyObjectByType<SceneLoader>();
            }
        }

        public void ClickImage()
        {
            _sceneLoader.LoadScene(nextScene);
        }

        public void EnterAnimation()
        {
            //transform.DOScale()
        }

        public void ExitAnimation()
        {
            
        }
    }
}