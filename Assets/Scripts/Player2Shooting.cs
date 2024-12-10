using UnityEngine;

public class Player2Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    private PlayerMovement playerMovement;

    public Vector2 upOffset = new Vector2(0, 0);
    public Vector2 downOffset = new Vector2(0, 0);
    public Vector2 leftOffset = new Vector2(0, 0);
    public Vector2 rightOffset = new Vector2(0, 0);

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);

        Bullet bScript = bullet.GetComponent<Bullet>();
        bScript.shooter = gameObject;
        bScript.bulletTag = "PlayerBullet";

        Vector2 shootDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            shootDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            shootDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            shootDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            shootDirection = Vector2.right;
        }

        if (shootDirection != Vector2.zero)
        {
            bullet.transform.up = shootDirection; 
        }
    }
}
