using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.WJ.Core.Game;

namespace Assets.Scripts.WJ.Core.UI
{
    public class WJGameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI leftScoreText;
        [SerializeField] private TextMeshProUGUI rightScoreText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI winnerText;

        private void OnEnable()
        {
            WJScoreManager.OnScoreChanged += UpdateScoreUI;
            WJScoreManager.OnGameOver += ShowGameOver;
        }

        private void OnDisable()
        {
            WJScoreManager.OnScoreChanged -= UpdateScoreUI;
            WJScoreManager.OnGameOver -= ShowGameOver;
        }

        private void UpdateScoreUI(int leftScore, int rightScore)
        {
            leftScoreText.text = leftScore.ToString();
            rightScoreText.text = rightScore.ToString();
        }

        private void ShowGameOver(bool leftPlayerWon, string reason)
        {
            gameOverPanel.SetActive(true);
            winnerText.text = $"{(leftPlayerWon ? "左边" : "右边")}玩家获胜!\n原因: {reason}";
        }
    }
} 