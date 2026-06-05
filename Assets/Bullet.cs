using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float speed;

    [SerializeField]
    private float lifeTime = 5f;


    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
        Destroy(gameObject, lifeTime);
    }
}
