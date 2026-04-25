using Core.Dependencies;
using R3;
using System;
using UI;
using UnityEngine;
using Work.JES.Code.Systems;

namespace InfinityMode
{
    public class InfinityModeManager : MonoBehaviour
    {
        private static string _scoreKey = "InfinityModeScore";

        [SerializeField] private TimerModelSO timerModel;
        [SerializeField] private ResourceModelSO resourceModel;
        [SerializeField] private PopupModelSO popupModel;

        [Inject] private SceneLoader _sceneLoader;

        private void Awake()
        {
            timerModel.ResetProperty();
            resourceModel.ResetProperty();

            popupModel.SetPopup(
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
            resourceModel.Health
                .Where(health => health <= 0)
                .Subscribe(Popup())
                .AddTo(this);
        }

        private Action<int> Popup()
        {
            return _ =>
            {
                Time.timeScale = 0f;

                int minutes = Mathf.FloorToInt(timerModel.Timer.Value / 60);
                int seconds = Mathf.FloorToInt(timerModel.Timer.Value % 60);

                popupModel.SetPopup(
                    "결과",
                    $"이번 기록 : {minutes:D2}:{seconds:D2}",
                    "스테이지",
                    LabelColor.Blue,
                    ButtonColor.Mint,
                    () => RestartGame(),
                    PlayerPrefs.GetInt(_scoreKey, 0) < timerModel.Timer.Value
                );
                PlayerPrefs.SetInt(_scoreKey, Mathf.FloorToInt(timerModel.Timer.Value));
            };
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;
            _sceneLoader.LoadScene("SelectScene");
        }

        private void Update()
        {
            timerModel.Timer.Value += Time.deltaTime;
        }
    }
}