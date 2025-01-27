using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private float restartDistance = 1f; // Distance at which to restart the scene

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Check if the projectile is within the restart distance of the player
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= restartDistance)
        {
            // Restart the scene if projectile gets too close to the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject); // Destroy the projectile upon restart
        }
    }
}
