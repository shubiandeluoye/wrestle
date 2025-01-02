using Fusion;
using UnityEngine;

namespace Assets.Scripts.WJ.Core.Network
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 movement;
        public NetworkButtons buttons;
    }

    public static class NetworkInputButtons
    {
        public const int Shoot = 1;
        public const int UpShoot = 2;
        public const int DownShoot = 3;
        public const int SwitchAngle = 4;
        public const int SwitchBullet = 5;
    }
} 