using System;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(EnemyMovement), (typeof(RangeEnemyAttack)))]
public class RangeEnemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement movement;
    private RangeEnemyAttack attack;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header("Elements")]
    private Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] private float playerDetectionRadius;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    private void Start()
    {
        health = maxHealth;

        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<RangeEnemyAttack>();

        player = FindFirstObjectByType<Player>();

        attack.StorePlayer(player);

        if (player == null)
        {
            Debug.LogWarning("No Player Found");
            Destroy(gameObject);
        }

        StartSpawnSequence();
    }

    void Update()
    {
        if(!spriteRenderer.enabled)
            return;

        ManageAttack();
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }

    private void SpawnSequnceCompleted()
    {
        SetRenderersVisibility(true);
        hasSpawned = true;

        collider.enabled = true;

        movement.StorePlayer(player);
    }

    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false);

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequnceCompleted);
    }

    private void SetRenderersVisibility(bool visibility = true)
    {
        spriteRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    private void TryAttack()
    {
        attack.AutoAim();
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
        {
            PassAway();
        }
    }
}
