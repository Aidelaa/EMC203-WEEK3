using UnityEngine;
using UnityEngine.SceneManagement;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private float rotationSpeed = 5f; // Rotation speed of the turret
    [SerializeField] private float fireRange = 10f; // Range to detect the player
    [SerializeField] private float fireCooldown = 1f; // Time cooldown between shots
    [SerializeField] private float firingAngleThreshold = 10f; // Angle threshold for firing

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectileP; // Projectile to be fired
    [SerializeField] private Transform firePoint; // Point where the projectile is fired from
    [SerializeField] private float projectileSpeed = 10f; // Speed of the projectile

    private Transform playerTransform; // Cached player transform
    private float fireTime = 0f; // Time when the turret last fired

    private void Start()
    {
        // Cache the player's transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player object has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        TurnTowardsPlayer();

        if (FireAtPlayer())
        {
            Attack();
        }
    }

    private void Turn()
    {
        // Calculate the direction to the player and rotate the turret
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool FireAtPlayer()
    {
        // Check if the player is within range, within the firing angle, and cooldown has elapsed
        return PlayerInRange() && FiringAngle() && Time.time >= fireTime + fireCooldown;
    }

    private bool PlayerInRange()
    {
        // Check if the player is within the firing range
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        return distance <= fireRange;
    }

    private bool FiringAngle()
    {
        // Check if the player is within the specified firing angle
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        float turretAngle = transform.eulerAngles.z;
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(turretAngle, angleToPlayer));

        return angleDifference <= firingAngleThreshold;
    }

    private void Attack()
    {
        // Instantiate and fire the projectile
        GameObject projectile = Instantiate(projectileP, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed; // Apply velocity to the projectile
        }

        fireTime = Time.time; // Update the last fire time
    }
}
