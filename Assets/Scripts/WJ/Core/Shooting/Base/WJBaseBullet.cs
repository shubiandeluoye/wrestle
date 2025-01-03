using UnityEngine;

public class WJBaseBullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;
    private int shooterId;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(float angle, int shooterId)
    {
        this.shooterId = shooterId;
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.right;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    public int GetShooterId()
    {
        return shooterId;
    }
}
