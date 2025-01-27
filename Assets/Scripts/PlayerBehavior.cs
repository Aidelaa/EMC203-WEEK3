using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Movement speed

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 input; // Stores input values

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Cache the Rigidbody2D component
    }

    private void Update()
    {
        GetInput(); // Capture player input
    }

    private void FixedUpdate()
    {
        Move(); // Apply movement in FixedUpdate for physics consistency
    }

    // Captures movement input
    private void GetInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or arrow keys)
        float moveY = Input.GetAxisRaw("Vertical"); // Get vertical input (W/S or arrow keys)
        

        input = new Vector2(moveX, moveY).normalized; // Normalize to prevent faster diagonal movement
    }

    // Handles player movement
    private void Move()
    {
        rb.linearVelocity = input * speed; // Set velocity based on input and speed
    }
}
