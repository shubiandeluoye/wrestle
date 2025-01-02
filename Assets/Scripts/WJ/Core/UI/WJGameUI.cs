using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.WJ.Core.Game;

namespace Assets.Scripts.WJ.Core.UI
{
    public class WJGameUI : MonoBehaviour
    {
        [Header("Score Display")]
        [SerializeField] private TextMeshProUGUI leftScoreText;
        [SerializeField] private TextMeshProUGUI rightScoreText;
        
        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI winnerText;

        private void OnEnable()
        {
            WJNetworkScoreManager.OnScoreChanged += UpdateScoreDisplay;
            WJNetworkScoreManager.OnGameOver += ShowGameOver;
        }

        private void OnDisable()
        {
            WJNetworkScoreManager.OnScoreChanged -= UpdateScoreDisplay;
            WJNetworkScoreManager.OnGameOver -= ShowGameOver;
        }

        private void UpdateScoreDisplay(int leftScore, int rightScore)
        {
            if (leftScoreText) leftScoreText.text = leftScore.ToString();
            if (rightScoreText) rightScoreText.text = rightScore.ToString();
        }

        private void ShowGameOver(bool leftPlayerWon, string reason)
        {
            if (gameOverPanel) gameOverPanel.SetActive(true);
            if (winnerText) 
            {
                string winner = leftPlayerWon ? "左边" : "右边";
                winnerText.text = $"{winner}玩家获胜！\n原因：{reason}";
            }
        }
    }
} 