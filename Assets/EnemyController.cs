using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("プレハブ設定")]
    public GameObject enemyBulletPrefab;

    [Header("HP設定")]
    public int maxHp = 300;             
    private int currentHp;

    [Header("弾幕モード設定")]
    public float patternInterval = 5f;
    private int currentPattern = 0;
    private float patternTimer;
    private float shotTimer;

    private GameObject player;
    private float spiralAngle01 = 0f;
    private float spiralAngle02 = 180f;
    private int waveCount = 0;

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
            waveCount = 0;
        }

        ExecutePattern(currentPattern);
    }

    void ExecutePattern(int pattern)
    {
        shotTimer += Time.deltaTime;

        switch (pattern)
        {
            case 0:
                if (shotTimer >= 0.03f) 
                {
                    ShootDoubleSpiral();
                    shotTimer = 0f;
                }
                break;

            case 1:
                if (shotTimer >= 0.15f)
                {
                    ShootCrossRing();
                    shotTimer = 0f;
                }
                break;

            case 2:
                if (shotTimer >= 0.08f)
                {
                    ShootWinderAndSlowingRing();
                    shotTimer = 0f;
                }
                break;
        }
    }
    void CreateBullet(Vector2 direction, float speed)
    {
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = speed;
            bulletScript.SetDirection(direction);
        }
    }

    void ShootDoubleSpiral()
    {
        for (int i = 0; i < 4; i++)
        {
            float angle = spiralAngle01 + (i * 90f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 5f);
        }
        for (int i = 0; i < 4; i++)
        {
            float angle = spiralAngle02 - (i * 90f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 4.5f); 
        }

        spiralAngle01 += 11f; 
        spiralAngle02 -= 7f;  
    }

    void ShootCrossRing()
    {
        int count = 16;
        float angleStep = 360f / count;
        float offsetAngle = (waveCount % 2 == 0) ? 0f : (angleStep / 2f);

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep) + offsetAngle;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            CreateBullet(dir, 3.5f);
            CreateBullet(dir, 5.5f);
        }
        waveCount++;
    }
    private float winderTimer = 0f;
    void ShootWinderAndSlowingRing()
    {
        if (player == null) return;

        winderTimer += Time.deltaTime;
        Vector2 toPlayer = player.transform.position - transform.position;
        float playerAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        float leftWall = playerAngle - 12f;
        float rightWall = playerAngle + 12f;

        Vector2 leftDir = new Vector2(Mathf.Cos(leftWall * Mathf.Deg2Rad), Mathf.Sin(leftWall * Mathf.Deg2Rad));
        Vector2 rightDir = new Vector2(Mathf.Cos(rightWall * Mathf.Deg2Rad), Mathf.Sin(rightWall * Mathf.Deg2Rad));

        CreateBullet(leftDir, 8f);
        CreateBullet(rightDir, 8f);
        if (winderTimer >= 0.6f)
        {
            int count = 20;
            float step = 360f / count;
            for (int i = 0; i < count; i++)
            {
                float angle = i * step;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                CreateBullet(dir, 2f);
            }
            winderTimer = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            currentHp--;

            if (currentHp <= 0)
            {
                Debug.Log("ゲームクリア！");
                Destroy(gameObject);
            }
        }
    }
}