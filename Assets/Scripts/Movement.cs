using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    void Start()
    {
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        
        playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
       
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
       
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        transform.position = newPosition;
    }
}
