using Core.Dependencies;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Work.JES.Code.Systems
{
    [Provide]
    public class SceneLoader : MonoBehaviour,IDependencyProvider
    {
        [Provide] public SceneLoader GetSceneLoader() => this;
        [SerializeField] private float loadDelay = 1f;
        private Animator _animator;
        private readonly int _loadSceneHash = Animator.StringToHash("LoadScene");

        [SerializeField] private Image[] barImages;
        [SerializeField] private RectTransform inRectTransform;
        private readonly int _fadeShaderId = Shader.PropertyToID("_Power");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            FadeIn();
        }
        public async void LoadScene(string sceneName)
        {
            if (_animator == null)
            {
                Debug.LogError("Animator component is not initialized.");
                return;
            }
            
            _animator.SetTrigger(_loadSceneHash);
            await Awaitable.WaitForSecondsAsync(loadDelay);
            SceneManager.LoadScene(sceneName);
        }

        private async void FadeIn()
        {
            foreach (var barImage in barImages)
            {
                barImage.material = new  Material(barImage.material);
                barImage.material.DOFloat(0.6f, _fadeShaderId, 0.2f).OnComplete(() =>
                    inRectTransform.DOAnchorPosY(inRectTransform.localPosition.y - 180, 0.1f));
                await Awaitable.WaitForSecondsAsync(0.3f);
            }
        }
    }
}