using UnityEngine;
using UnityEngine.UI;

public class WJGameUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    public void UpdateScore(int player1Score, int player2Score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{player1Score} - {player2Score}";
        }
    }
} 