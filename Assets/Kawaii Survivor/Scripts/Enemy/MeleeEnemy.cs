using System;
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
            
            if (attackTimer >= attackDelay)
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
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= playerDetectionRadius)
                Attack();
        
        }

        private void Attack()
        {
            attackTimer = 0f;

            player.TakeDamage(damage);
        }
    }
}
