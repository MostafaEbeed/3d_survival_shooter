using System.Collections.Generic;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State
    {
        Idle,
        Attack
    }

    private State state;
    
    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header("Settings")]
    private List<Enemy> damagedEnemies = new List<Enemy>();

    
    void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attack:
                Attacking();
                break;
        }
    }
    
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if(closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;

            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncreamentAttackTimer();
    }

    private void ManageAttack()
    {
        if(attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncreamentAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    
    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;

        damagedEnemies.Clear();

        animator.speed = 1f / attackDelay;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll
        (
            hitDetectionTransform.position,
            hitCollider.bounds.size,
            hitDetectionTransform.transform.eulerAngles.z,
            enemyMask
        );

        for(int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            if(enemy)
            {
                if (!damagedEnemies.Contains(enemy))
                {
                    int damage = GetDamage(out bool isCritical);
                    
                    enemy.TakeDamage(damage, isCritical);
                    damagedEnemies.Add(enemy);
                }
            }
        }
    }

    public override void UpdateStat(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        
        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100f);
        
        criticalChance = (int)(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100f));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);
    }
}
