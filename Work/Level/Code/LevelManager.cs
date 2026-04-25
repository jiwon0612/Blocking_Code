using Core.Dependencies;
using Events;
using EventSystems;
using R3;
using UI;
using UnityEngine;
using Work.JES.Code.Systems;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [Inject]
        private SceneLoader _sceneLoader;

        [SerializeField] private ResourceModelSO resourceModelSO;
        [SerializeField] private PopupModelSO popupModelSO;
        [SerializeField] private GameEventChannelSO cameraChannel;

        private void Awake()
        {
            resourceModelSO.ResetProperty();
            popupModelSO.SetPopup(
                null,
                null,
                null,
                LabelColor.Blue,
                ButtonColor.Gray,
                () => { }
            );
        }

        private void Start()
        {
            resourceModelSO.Health
                .Subscribe(health =>
                {
                    if (health <= 0)
                    {
                        Time.timeScale = 0f;
                        popupModelSO.SetPopup(
                            "패배",
                            "모든 생명을 잃었습니다!",
                            "스테이지",
                            LabelColor.Red,
                            ButtonColor.Navy,
                            () => { Time.timeScale = 1f; _sceneLoader.LoadScene("SelectScene"); }
                        );
                    }
                })
                .AddTo(this);
            resourceModelSO.Health
                .Subscribe(health => cameraChannel.RaiseEvent(CameraEvent.ImpulseEvent))
                .AddTo(this);
        }
    }
}