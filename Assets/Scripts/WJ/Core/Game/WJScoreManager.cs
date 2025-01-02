using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJScoreManager : MonoBehaviour
    {
        [System.Serializable]
        public class ScoreData
        {
            public int currentScore = 0;
            public int targetScore = 5;  // 获胜所需分数
        }

        [Header("Score Settings")]
        [SerializeField] private ScoreData leftPlayerScore = new ScoreData();
        [SerializeField] private ScoreData rightPlayerScore = new ScoreData();
        
        [Header("Events")]
        public UnityEvent<int, int> onScoreChanged;  // 参数：左玩家分数，右玩家分数
        public UnityEvent<bool> onGameOver;          // 参数：左玩家是否获胜

        public static WJScoreManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddScore(bool isLeftPlayer, int score = 1)
        {
            ScoreData playerScore = isLeftPlayer ? leftPlayerScore : rightPlayerScore;
            playerScore.currentScore += score;
            
            Debug.Log($"[Score] {(isLeftPlayer ? "Left" : "Right")} player scored! Current: {playerScore.currentScore}");
            
            // 触发分数变化事件
            onScoreChanged?.Invoke(leftPlayerScore.currentScore, rightPlayerScore.currentScore);
            
            // 检查是否达到胜利条件
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            bool leftPlayerWon = leftPlayerScore.currentScore >= leftPlayerScore.targetScore;
            bool rightPlayerWon = rightPlayerScore.currentScore >= rightPlayerScore.targetScore;

            if (leftPlayerWon || rightPlayerWon)
            {
                Debug.Log($"[Score] Game Over! {(leftPlayerWon ? "Left" : "Right")} player won!");
                onGameOver?.Invoke(leftPlayerWon);
            }
        }

        public void ResetScores()
        {
            leftPlayerScore.currentScore = 0;
            rightPlayerScore.currentScore = 0;
            onScoreChanged?.Invoke(0, 0);
            Debug.Log("[Score] Scores reset");
        }

        public (int left, int right) GetCurrentScores()
        {
            return (leftPlayerScore.currentScore, rightPlayerScore.currentScore);
        }
    }
} 