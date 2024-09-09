using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // You can adjust this value in the Unity Inspector

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(moveX, moveY) * speed * Time.deltaTime);
    }
}
