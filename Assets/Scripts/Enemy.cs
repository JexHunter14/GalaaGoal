using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f; 
    public float shootCooldown = 1f;  
    public GameObject bulletPrefab;   
    public Transform firePoint;       
    private Transform player;          
    private float nextShootTime = 0f;  

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            TryShoot();
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

    void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bullet.GetComponent<Bullet>().speed;
        bullet.transform.up = direction;
    }
}
