using UnityEngine;
using System;
using Assets.Scripts.WJ.Core.Audio;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJScoreManager : MonoBehaviour
    {
        private int leftPlayerScore = 99;
        private int rightPlayerScore = 99;
        private bool isGameOver;
        [SerializeField] private int minScore = 0;

        public static event Action<int, int> OnScoreChanged;
        public static event Action<bool, string> OnGameOver;

        private void Start()
        {
            ResetScore();
        }

        public void DeductScore(bool isLeftPlayer)
        {
            if (isLeftPlayer)
            {
                leftPlayerScore = Mathf.Max(minScore, leftPlayerScore - 1);
            }
            else
            {
                rightPlayerScore = Mathf.Max(minScore, rightPlayerScore - 1);
            }

            OnScoreChanged?.Invoke(leftPlayerScore, rightPlayerScore);
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            if (leftPlayerScore <= minScore || rightPlayerScore <= minScore)
            {
                isGameOver = true;
                bool leftPlayerWon = rightPlayerScore <= minScore;
                OnGameOver?.Invoke(leftPlayerWon, "分数耗尽");
                WJAudioManager.Instance?.PlayGameOverSound();
            }
        }

        public void ResetScore()
        {
            leftPlayerScore = 99;
            rightPlayerScore = 99;
            isGameOver = false;
            OnScoreChanged?.Invoke(99, 99);
        }

        public (int left, int right) GetCurrentScore()
        {
            return (leftPlayerScore, rightPlayerScore);
        }

        public void PlayerOutOfBounds(WJPlayerController player)
        {
            if (!isGameOver)
            {
                isGameOver = true;
                OnGameOver?.Invoke(!player.GetIsLeftPlayer(), "对手掉出地图");
                WJAudioManager.Instance?.PlayGameOverSound();
            }
        }
    }
} 