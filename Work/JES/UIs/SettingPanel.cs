using System.Collections.Generic;
using Core.Dependencies;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Work.JES.Code.Systems;

namespace Work.JES.UIs
{
    public class SettingPanel : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        [SerializeField] private InputSO playerInput;
        [SerializeField] private CanvasGroup canvasGroup;
        private bool _isOn = false;
        private void Start()
        {
            if(playerInput!=null)
                playerInput.OnEscPressed += HandleEsc;
            OnOffCanvasGroup(false);
        }

        private void HandleEsc()
        {
            // Esc 키가 눌렸을 때 캔버스 그룹의 활성화 상태를 토글합니다.
            OnOffCanvasGroup(!_isOn);
        }

        public void ReLoadScene()
        {
            OnOffCanvasGroup(false);
            // 현재 씬을 다시 로드합니다.
            SceneManager.LoadScene("SelectScene");
        }
        public void OnSettingPanel()=>
            OnOffCanvasGroup(true);

        private void OnDestroy()
        {
            // 드롭다운 이벤트 리스너 제거
            if(playerInput!=null)
                playerInput.OnEscPressed -= HandleEsc;
        }
        public void SettingExitBtnClick()
        {
            OnOffCanvasGroup(false);
        }
        public void GoLTitleBtnClick()
        {
            OnOffCanvasGroup(false);
            _sceneLoader.LoadScene("Title");
        }
        private void OnOffCanvasGroup(bool isOn)
        {
            _isOn = isOn;
            Time.timeScale = isOn ? 0 : 1;
            canvasGroup.alpha = isOn ? 1 : 0;
            canvasGroup.interactable = isOn;
            canvasGroup.blocksRaycasts = isOn;
        }
    }
}