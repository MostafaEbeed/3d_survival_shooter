using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kawaii_Survivor.Scripts.Enemy
{
    [RequireComponent (typeof(EnemyMovement))]
    public class MeleeEnemy : Enemy
    {
        [Header("Attack")]
        [SerializeField] private int damage;
        [SerializeField] private float attackFrequency;
        [SerializeField] private float animationSyncDelay;
        private float attackDelay;
        private float attackTimer;

        protected override void Start()
        {
            base.Start();
            
            attackDelay = 1f / attackFrequency;
        }

        void Update()
        {
            if(!CanAttack()) return;
            
            if (attackTimer >= attackFrequency)
                TryAttack();
            else
                Wait();

            movement.FollowPlayer();
        }
        
        private void Wait()
        {
            attackTimer += Time.deltaTime;
        }

        private void TryAttack()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= playerDetectionRadius)
                Attack();
        
        }

        private void Attack()
        {
            animationController.PlayAttackAnimation();
            
            attackTimer = 0f;

            StartCoroutine(CastDamage());
        }

        IEnumerator CastDamage()
        {
            yield return new WaitForSeconds(animationSyncDelay);
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerDetectionRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    player.TakeDamage(damage);
                    yield return null;
                }
            }
        }
    }
}
