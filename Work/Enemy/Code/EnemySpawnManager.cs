using UI;
using UnityEngine;
using Core.Dependencies;
using Work.JES.Code.Systems;

namespace Enemies
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [Inject]
        private SceneLoader _sceneLoader;
        [SerializeField] private PopupModelSO popupModelSO;

        private int _spawnerCount = 0;
        private int _endCount = 0;

        private void Start()
        {
            var spawners = GetComponentsInChildren<EnemySpawner>();
            _spawnerCount = spawners.Length;
            foreach (var spawner in spawners)
            {
                spawner.OnSpawnEnd += HandleSpawnEndEvent;
            }
        }

        private void HandleSpawnEndEvent()
        {
            _endCount++;
            if (_endCount >= _spawnerCount)
            {
                Time.timeScale = 0f;
                popupModelSO.SetPopup(
                    "승리",
                    "모든 적을 처치했습니다!",
                    "스테이지",
                    LabelColor.Orange,
                    ButtonColor.Green,
                    () => { Time.timeScale = 1f; _sceneLoader.LoadScene("SelectScene"); }
                );
            }
        }
    }
}