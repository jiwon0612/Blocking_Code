using UnityEngine;

public class BestScoreText : MonoBehaviour
{
    private static string _scoreKey = "InfinityModeScore";

    [SerializeField] private TMPro.TextMeshProUGUI bestScoreText;
    private void Start()
    {
        UpdateBestScoreText();
    }
    private void UpdateBestScoreText()
    {
        int minutes = PlayerPrefs.GetInt(_scoreKey, 0) / 60;
        int seconds = PlayerPrefs.GetInt(_scoreKey, 0) % 60;
        bestScoreText.text = $"최고 기록 : {minutes:D2}:{seconds:D2}";
    }
}
