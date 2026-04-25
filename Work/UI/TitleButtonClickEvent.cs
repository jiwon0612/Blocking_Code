using Core.Dependencies;
using Unity.VisualScripting;
using UnityEngine;
using Work.JES.Code.Systems;

namespace UI
{
    public class TitleButtonClickEvent : MonoBehaviour
    {
        [SerializeField] string nextScene;
        [SerializeField] Transform settingPannelTrm;
        
        private RectTransform SettingPannelTrm => settingPannelTrm as RectTransform;

        [Inject] private SceneLoader _sceneLoader; 

        public void OnStart()
        {
            _sceneLoader.LoadScene(nextScene);
        }
        
        public void OnSetting()
        {
            
        }
        
        public void OnQuit() => Application.Quit();
    }
}