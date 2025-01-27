using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private float rotationSpeed = 5f; // Speed at which the turret rotates
    [SerializeField] private float detectionRange = 10f; // Distance within which the turret detects the player
    [SerializeField] private float shotCooldown = 1f; // Time delay between shots
    [SerializeField] private float firingAngleLimit = 10f; // Angle range within which the turret can fire

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile
    [SerializeField] private Transform projectileSpawnPoint; // Spawn point for the projectile
    [SerializeField] private float projectileVelocity = 10f; // Speed of the projectile

    private Transform player; // Reference to the player's transform
    private float lastShotTime = 0f; // Time when the turret last fired

    private void Start()
    {
        // Locate the player in the scene and cache its transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        RotateTowardsPlayer();

        if (CanShootAtPlayer())
        {
            FireProjectile();
        }
    }

    private void RotateTowardsPlayer()
    {
        // Calculate direction to the player and smoothly rotate the turret
        Vector2 direction = player.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsPlayerInRange()
    {
        // Check if the player is within detection range
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        return distanceToPlayer <= detectionRange;
    }

    private bool IsPlayerWithinFiringAngle()
    {
        // Check if the player is within the firing angle
        Vector2 direction = player.position - transform.position;
        float playerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float turretAngle = transform.eulerAngles.z;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(turretAngle, playerAngle));

        return angleDifference <= firingAngleLimit;
    }

    private bool CanShootAtPlayer()
    {
        // Determine if the turret can shoot based on range, angle, and cooldown
        return IsPlayerInRange() && IsPlayerWithinFiringAngle() && Time.time >= lastShotTime + shotCooldown;
    }

    private void FireProjectile()
    {
        // Spawn and fire a projectile towards the player
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 fireDirection = (player.position - transform.position).normalized;
            rb.velocity = fireDirection * projectileVelocity; // Apply velocity to the projectile
        }

        lastShotTime = Time.time; // Update the cooldown timer
    }
}
