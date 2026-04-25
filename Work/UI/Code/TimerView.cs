using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        public void UpdateTimer(float time)
        {
            if (timerText != null)
            {
                timerText.text = FormatTime(time);
            }
        }
        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}