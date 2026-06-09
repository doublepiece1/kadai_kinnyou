using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector2 moveInput;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform firePoint;
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Moving: {moveInput}");
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Fire");

            Shoot();
        }
    }

    public void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Debug.Log("ゲームオーバー！");
            Destroy(gameObject); 
        }
    }
}

