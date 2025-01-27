using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Rotation speed of the turret
    [SerializeField] private float fireRange = 10f; // Range to detect the player
    [SerializeField] private float fireCooldown = 1f; // Time cooldown between shots
    [SerializeField] private GameObject projectilePrefab; // Projectile to be fired
    [SerializeField] private Transform firePoint; // Point where the projectile is fired from
    [SerializeField] private float projectileSpeed = 10f; // Speed of the projectile
    [SerializeField] private float restartDistance = 1f; // Distance at which to restart the scene
    [SerializeField] private float firingAngleThreshold = 10f; // Angle threshold for firing

    private float lastFireTime = 0f; // Last time the turret fired

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Turn turret towards the player
        TurnTowardsPlayer(player);

        // Check if the player is within firing range and directly in front of the turret
        if (IsPlayerInRange(player) && IsPlayerInFiringAngle(player) && Time.time >= lastFireTime + fireCooldown)
        {
            FireAtPlayer(player);
        }
    }

    private void TurnTowardsPlayer(GameObject player)
    {
        // Calculate the direction to the player
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Adjust for 2D rotation (Z-axis)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsPlayerInRange(GameObject player)
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        return distance <= fireRange;
    }

    private bool IsPlayerInFiringAngle(GameObject player)
    {
        // Check if the player is within the specified firing angle
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        float turretAngle = transform.eulerAngles.z;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(turretAngle, angleToPlayer));

        return angleDifference <= firingAngleThreshold;
    }

    private void FireAtPlayer(GameObject player)
    {
        // Fire a projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed; // Using velocity instead of linearVelocity
        }

        lastFireTime = Time.time; // Reset fire cooldown
    }
}
