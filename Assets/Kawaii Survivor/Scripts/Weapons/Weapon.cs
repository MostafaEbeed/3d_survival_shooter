using System;
using System.Collections.Generic;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO weaponData { get; private set; }
    
    [Header("Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;

    [Header("Attack")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
 
    [SerializeField] protected Animator animator;

    protected float attackTimer;

    [Header("Critical")]
    protected float criticalChance;
    protected float criticalPercent;
    
    [Header("Animations")]
    [SerializeField] protected float aimLerp;

    [Header("Level")]
    [field: SerializeField] public int Level { get; private set; }
    
    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;
            return (int)(damage * criticalPercent);
        }
        
        return damage;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Vector2 targetUpVector = Vector3.up;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(transform.position, range);  
    }

    public abstract void UpdateStat(PlayerStatsManager playerStatsManager);

    protected void ConfigureStats()
    {
        float multiplier = 1 + (float)Level / 3;
        
        damage = Mathf.RoundToInt(weaponData.GetStatValue(Stat.Attack) * multiplier);
        
        attackDelay = 1f / (weaponData.GetStatValue(Stat.AttackSpeed) * multiplier);
        
        criticalChance = (int)weaponData.GetStatValue(Stat.CriticalChance) * multiplier;
        criticalPercent = weaponData.GetStatValue(Stat.CriticalPercent) * multiplier;
        
        if(weaponData.Prefab.GetType() == typeof(RangeWeapon))
            range = weaponData.GetStatValue(Stat.Range) * multiplier;
    }
}
