using UnityEngine;

namespace WJ.Core.Movement.Data
{
    [CreateAssetMenu(fileName = "WJMovementData", menuName = "WJ/Movement/MovementData")]
    public class WJMovementData : ScriptableObject
    {
        [Header("Basic Movement")]
        [Tooltip("基础移动速度")]
        public float moveSpeed = 5f;
        [Tooltip("加速度")]
        public float acceleration = 50f;
        [Tooltip("减速度")]
        public float deceleration = 50f;

        [Header("Ground Detection")]
        [Tooltip("地面检测距离")]
        public float groundCheckDistance = 0.2f;
        [Tooltip("地面标签")]
        public string groundTag = "Floor";

        [Header("Advanced Movement")]
        [Tooltip("最大速度")]
        public float maxSpeed = 8f;
        [Tooltip("转向速度")]
        public float rotationSpeed = 10f;
        [Tooltip("空中移动速度系数")]
        public float airControlFactor = 0.5f;
    }
}