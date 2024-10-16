using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;

    [Header("Spawn Sequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Header")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Debug")]
    [SerializeField] private bool displayGizmos;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        
        if(player == null)
        {
            Debug.LogWarning("No Player Found");
            Destroy(gameObject);
        }

        spriteRenderer.enabled = false;

        spawnIndicator.enabled = true;

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequnceCompleted);
    }

    void Update()
    {
        if(!hasSpawned) return;

        FollowPlayer();

        TryAttack();
    }

    private void SpawnSequnceCompleted()
    {
        spriteRenderer.enabled = true;

        spawnIndicator.enabled = false;

        hasSpawned = true;
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= playerDetectionRadius)
            PassAway();
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

        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
