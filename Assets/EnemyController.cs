using UnityEngine;



public class EnemyController : MonoBehaviour
{
    [Header("プレハブ設定")]
    public GameObject enemyBulletPrefab;

    [Header("HP設定")]
    public int maxHp = 100;
    private int currentHp;

    [Header("弾幕モード設定")]
    public float patternInterval = 4f; 
    private int currentPattern = 0;    
    private float patternTimer;
    private float shotTimer;

    private GameObject player;

    private float spiralAngle = 0f;

    void Start()
    {
        currentHp = maxHp;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        patternTimer += Time.deltaTime;
        if (patternTimer >= patternInterval)
        {
            currentPattern = (currentPattern + 1) % 3; 
            patternTimer = 0f;
            shotTimer = 0f; 
        }
        ExecutePattern(currentPattern);
    }

    void ExecutePattern(int pattern)
    {
        shotTimer += Time.deltaTime;

        switch (pattern)
        {
            case 0:
                if (shotTimer >= 0.4f)
                {
                    ShootTargeted3Way();
                    shotTimer = 0f;
                }
                break;
            case 1:

                if (shotTimer >= 0.05f) 
                {
                    ShootSpiral();
                    shotTimer = 0f;
                }
                break;

            case 2:
                if (shotTimer >= 1.0f)
                {
                    ShootRingAndSnipe();
                    shotTimer = 0f;
                }
                break;
        }
    }
    void CreateBullet(Vector2 direction, float speed = 5f)
    {
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = speed;
            bulletScript.SetDirection(direction);
        }
    }
    void ShootTargeted3Way()
    {
        if (player == null) return;
        Vector2 toPlayer = player.transform.position - transform.position;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        float[] angles = { baseAngle, baseAngle - 15f, baseAngle + 15f };
        foreach (float angle in angles)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 7f); 
        }
    }

    void ShootSpiral()
    {
        int ways = 4; 
        float step = 360f / ways;

        for (int i = 0; i < ways; i++)
        {
            float angle = spiralAngle + (i * step);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 4f); 
        }
        spiralAngle += 13f;
    }
    void ShootRingAndSnipe()
    {
        int count = 24;
        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 3f); 
        }
        if (player != null)
        {
            Vector2 toPlayer = (player.transform.position - transform.position).normalized;
            CreateBullet(toPlayer, 9f); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            currentHp--;
            Debug.Log("敵に命中！ 残りHP: " + currentHp);

            if (currentHp <= 0)
            {
                Debug.Log("ゲームクリア！");
                Destroy(gameObject);
            }
        }
    }
}
