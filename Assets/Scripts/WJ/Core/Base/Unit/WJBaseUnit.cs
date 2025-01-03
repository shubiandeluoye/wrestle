using UnityEngine;

public class WJBaseUnit : MonoBehaviour
{
    public const float UNIT_SIZE = 1f;
    public const float BASE_SPEED = 10f;
    public const float BASE_ROTATION_SPEED = 90f;
    public const float BASE_SCALE = 1f;

    public static readonly Vector3 ORIGIN_POSITION = Vector3.zero;
    public static readonly Vector3 UP_POSITION = Vector3.up * UNIT_SIZE * 10f;
    public static readonly Vector3 RIGHT_POSITION = Vector3.right * UNIT_SIZE * 10f;
    public static readonly Vector3 FORWARD_POSITION = Vector3.forward * UNIT_SIZE * 10f;
}
