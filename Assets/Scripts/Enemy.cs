using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f;
    public float shootCooldown = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;  
    private Transform player;
    private float nextShootTime = 0f;
    private bool canShoot = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
    }

    void Update()
    {
        if (canShoot)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                FacePlayer();
                TryShoot();
            }
        }
    }

    void TryShoot()
    {
        if (Time.time >= nextShootTime)
        {
            ShootAtPlayer();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    void FacePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        transform.up = directionToPlayer; 
    }
    void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bScript = bullet.GetComponent<Bullet>();
        bScript.shooter = gameObject;
        bScript.bulletTag = "EnemyBullet";

        Vector2 direction = (player.position - firePoint.position).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = direction * bullet.GetComponent<Bullet>().speed;
        bullet.transform.up = direction;
    }

    public void DisableShooting()
    {
        canShoot = false;
    }

    public void EnableShooting()
    {
        canShoot = true;
    }
}
