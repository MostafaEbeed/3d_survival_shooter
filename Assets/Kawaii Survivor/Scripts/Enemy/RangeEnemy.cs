using System;
using UnityEngine;

namespace Kawaii_Survivor.Scripts.Enemy
{
    [RequireComponent (typeof(EnemyMovement), (typeof(RangeEnemyAttack)))]
    public class RangeEnemy : Enemy
    {
        private RangeEnemyAttack attack;

        protected override void Start()
        {
            base.Start();
            
            attack = GetComponent<RangeEnemyAttack>();
            attack.StorePlayer(player);
        }

        void Update()
        {
            if(!CanAttack()) return;
            
            ManageAttack();
            
            transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
        }

        private void ManageAttack()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > playerDetectionRadius)
                movement.FollowPlayer();
            else
                TryAttack();
        }

        private void TryAttack()
        {
            attack.AutoAim();
        }
    }
}
