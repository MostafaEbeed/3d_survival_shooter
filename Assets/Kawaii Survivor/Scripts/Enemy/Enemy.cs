using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Kawaii_Survivor.Scripts.Enemy
{
    public abstract class Enemy : MonoBehaviour, IGameStateListener
    {
        [Header("Components")]
        protected EnemyMovement movement;
        [SerializeField] protected EnemyAnimationController animationController;
        
        [Header("Health")]
        [SerializeField] protected int maxHealth;
        protected int health;
        
        [Header("Elements")]
        protected Player player;
        
        [Header("Spawn Sequence Related")]
        [SerializeField] protected GameObject enemyVisual;
        [SerializeField] protected GameObject spawnIndicator;
        [FormerlySerializedAs("collider")] [SerializeField] protected Collider enemyCollider;
        protected bool hasSpawned;
        
        [Header("Effects")]
        [SerializeField] protected ParticleSystem passAwayParticles;

        [Header("Attack")]
        [SerializeField] protected float playerDetectionRadius;

        [Header("Actions")]
        public static Action<int, Vector2, bool> onDamageTaken;
        public static Action<Vector3> onPassedAway;

        [Header("Debug")]
        [SerializeField] protected bool displayGizmos;
        
        protected virtual void Start()
        {
            health = maxHealth;
            
            movement = GetComponent<EnemyMovement>();

            player = FindFirstObjectByType<Player>();

            if (player == null)
            {
                Debug.LogWarning("No Player Found");
                Destroy(gameObject);
            }
            
            StartSpawnSequence();
        }

        protected bool CanAttack()
        {
            return enemyVisual.activeInHierarchy;
        }
        
        private void StartSpawnSequence()
        {
            SetRenderersVisibility(false);

            Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
            LeanTween.scale(spawnIndicator.gameObject, targetScale, 0.3f)
                .setLoopPingPong(4)
                .setOnComplete(SpawnSequnceCompleted);
        }
        
        private void SpawnSequnceCompleted()
        {
            SetRenderersVisibility(true);
            hasSpawned = true;
            movement.enabled = true;
            animationController.StartAnimations();

            enemyCollider.enabled = true;

            movement.StorePlayer(GameManager.instance.Player, transform.position);
        }
        
        private void SetRenderersVisibility(bool visibility = true)
        {
            enemyVisual.SetActive(visibility);
            spawnIndicator.SetActive(!visibility);
        }
        
        public void TakeDamage(int damage, bool isCriticalHit)
        {
            int realDamage = Mathf.Min(damage, health);
            health -= realDamage;

            onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

            if (health <= 0)
            {
                PassAway();
            }
        }
        
        public void PassAway()
        {
            movement.enabled = false;
            
            movement.ClearPlayer();
            
            onPassedAway?.Invoke(transform.position);

            PassAwayAfterWave();
        }

        public void PassAwayAfterWave()
        {
            if(passAwayParticles == null) return;
            passAwayParticles.transform.SetParent(null);
            passAwayParticles.Play();

            if (gameObject)
            {
                LeanTween.cancel(gameObject);
                Destroy(gameObject);
            }
            
        }

        private void OnDrawGizmos()
        {
            if (!displayGizmos) return;

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
        }

        public void GameStateChangedCallback(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.STAGECOMPLETE:
                    PassAwayAfterWave();
                    break;
            
                case GameState.GAMEOVER:
                    PassAwayAfterWave();
                    break;
                
                case GameState.WAVETRANSITION:
                    PassAwayAfterWave();
                    break;
            }
        }
    }
}
