using UI;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Enemies;

namespace Tutorials
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("팝업 시스템 연결")]
        [SerializeField] private PopupModelSO popupModelSO;

        [Header("튜토리얼 섹션들")]
        [SerializeField] private List<TutorialSection> tutorialSections;

        [SerializeField] private EnemySpawner enemySpawner;

        public UnityEvent OnEndEvent;

        private int _currentSectionIndex;
        private int _currentTutorialIndex;
        private Tutorial _currentTutorial;

        private void Start()
        {
            enemySpawner.gameObject.SetActive(false);
            StartSection(0);
        }

        private void StartSection(int sectionIndex)
        {
            if (sectionIndex < 0 || sectionIndex >= tutorialSections.Count)
            {
                OnEndEvent?.Invoke();
                return;
            }

            var section = tutorialSections[sectionIndex];
            if (section == null || section.tutorials == null || section.tutorials.Count == 0)
            {
                OnEndEvent?.Invoke();
                return;
            }

            _currentSectionIndex = sectionIndex;
            _currentTutorialIndex = 0;
            _currentTutorial = section.tutorials[_currentTutorialIndex];
            ShowTutorialPopup();
        }

        private void ShowTutorialPopup()
        {
            var section = tutorialSections[_currentSectionIndex];
            string buttonText = (_currentTutorialIndex >= section.tutorials.Count - 1) ? "완료" : "다음";
            popupModelSO.SetPopup(
                section.sectionName,
                _currentTutorial.tutorialDescription,
                buttonText,
                LabelColor.Orange,
                ButtonColor.Green,
                OnTutorialPopupButtonClicked
            );
        }

        private void OnTutorialPopupButtonClicked()
        {
            _currentTutorial.OnComplete?.Invoke();
            NextTutorial();
        }

        public void NextTutorial()
        {
            var section = tutorialSections[_currentSectionIndex];
            if (_currentTutorialIndex >= section.tutorials.Count - 1)
                return;

            _currentTutorialIndex++;
            _currentTutorial = section.tutorials[_currentTutorialIndex];
            ShowTutorialPopup();
        }

        public void NextSection()
        {
            int nextSectionIndex = _currentSectionIndex + 1;
            if (nextSectionIndex < tutorialSections.Count)
            {
                StartSection(nextSectionIndex);
            }
            else
            {
                OnEndEvent?.Invoke();
            }
        }

        [ContextMenu("튜토리얼 테스트")]
        public void TutorialTest()
        {
            popupModelSO.SetPopup(
                "테스트",
                "테스트 튜토리얼입니다.",
                "확인",
                LabelColor.Orange,
                ButtonColor.Blue,
                () => Debug.Log("테스트 튜토리얼 완료")
            );
        }
    }
}
