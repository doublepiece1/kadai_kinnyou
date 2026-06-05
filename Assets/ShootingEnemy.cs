using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{

    [SerializeField] public GameObject _prefabBullet;

    [SerializeField]
    float _bulletVelocity = 2.0f;

    [SerializeField]
    float _intervel = 0.125f;

    [SerializeField]
    float _bulletLife = 10.0f;
    float _remaingTime = 0f;

    void Update()
    {
        _remaingTime -= Time.deltaTime;
        if (_remaingTime < 0.0f)
        {
            _remaingTime = _intervel;
            GameObject bullet = Instantiate(_prefabBullet);
            bullet.transform.position = transform.position;

            bullet.GetComponent<Rigidbody2D>().linearVelocity = NewBulletSpeed();
            Destroy(bullet, _bulletLife);
        }
    }

    Vector3 NewBulletSpeed()
    {
        float vx = Random.value * 2.0f - 1.0f;
        float vy = -Random.value;

        if (vx == 0.0f && vy == 0.0f)
        {
            vy = 1.0f;
        }
        Vector3 velocity = new Vector3(vx, vy, 0.0f);
        velocity.Normalize();
        velocity *= _bulletVelocity;
        return velocity;
    }
}