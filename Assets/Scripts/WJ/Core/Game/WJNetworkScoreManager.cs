using UnityEngine;
using Fusion;
using System;
using Assets.Scripts.WJ.Core.Audio;
using Assets.Scripts.WJ.Core.Player.Controllers;

namespace Assets.Scripts.WJ.Core.Game
{
    public class WJNetworkScoreManager : NetworkBehaviour
    {
        [Networked]
        private int LeftPlayerScore { get; set; } = 99;
        
        [Networked]
        private int RightPlayerScore { get; set; } = 99;
        
        [Networked]
        private NetworkBool IsGameOver { get; set; }

        [SerializeField] private int minScore = 0;

        // 事件系统用于UI更新
        public static event Action<int, int> OnScoreChanged;
        public static event Action<bool, string> OnGameOver;  // 参数：是否左玩家胜利，胜利原因

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                ResetScore();
            }
        }

        public void DeductScore(bool isLeftPlayer)
        {
            if (!Object.HasStateAuthority) return;

            if (isLeftPlayer)
            {
                LeftPlayerScore = Mathf.Max(minScore, LeftPlayerScore - 1);
            }
            else
            {
                RightPlayerScore = Mathf.Max(minScore, RightPlayerScore - 1);
            }

            // 通知所有客户端分数变化
            RPC_OnScoreChanged(LeftPlayerScore, RightPlayerScore);

            // 检查游戏结束条件
            CheckGameOver();
        }

        private void CheckGameOver()
        {
            if (!Object.HasStateAuthority) return;

            if (LeftPlayerScore <= minScore || RightPlayerScore <= minScore)
            {
                IsGameOver = true;
                bool leftPlayerWon = RightPlayerScore <= minScore;
                RPC_OnGameOver(leftPlayerWon, "分数耗尽");
            }
        }

        public void ResetScore()
        {
            if (!Object.HasStateAuthority) return;

            LeftPlayerScore = 99;
            RightPlayerScore = 99;
            IsGameOver = false;
            RPC_OnScoreChanged(99, 99);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnScoreChanged(int leftScore, int rightScore)
        {
            OnScoreChanged?.Invoke(leftScore, rightScore);
            Debug.Log($"[NetworkScore] Score updated - Left: {leftScore}, Right: {rightScore}");
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnGameOver(bool leftPlayerWon, string reason)
        {
            OnGameOver?.Invoke(leftPlayerWon, reason);
            WJAudioManager.Instance?.PlayGameOverSound();  // 播放游戏结束声音
            Debug.Log($"[NetworkScore] Game Over! {(leftPlayerWon ? "Left" : "Right")} player won! Reason: {reason}");
        }

        // 获取当前分数
        public (int left, int right) GetCurrentScore()
        {
            return (LeftPlayerScore, RightPlayerScore);
        }

        // 处理玩家掉出地图的情况
        public void PlayerOutOfBounds(WJPlayerController player)
        {
            if (player != null)
            {
                DeductScore(player.GetIsLeftPlayer());
            }
        }
    }
} 