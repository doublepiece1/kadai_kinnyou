using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
