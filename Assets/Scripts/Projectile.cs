using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float restartDistance = 1f; // Distance at which to restart the scene
    private Transform playerTransform; // Cached reference to the player's transform

     private void Start()
    {
        // Find the player by tag and cache its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return; // Exit if player is not found

        // Check if the projectile is within the restart distance of the player
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        if (distance <= restartDistance)
        {
            RestartScene();
        }
    }

    // Restarts the scene and destroys the projectile
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        Destroy(gameObject); // Destroy the projectile
    }
}
