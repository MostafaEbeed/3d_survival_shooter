using System;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootingPoint;
  
    [Header("Pooling")]
    private ObjectPool<Bullet> bulletPool;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    void Update()
    {
        AutoAim(); 
    }
    
    private Bullet CreateFunction()
    {
        Bullet bulletInstance = (Bullet)Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);

        return bulletInstance;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    
    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
    
        //Vector3 targetUpVector = Vector3.up;

        if(closestEnemy)
        {
            Debug.Log(closestEnemy.name);
            transform.LookAt(closestEnemy.transform);
            
            ManageShooting(closestEnemy);
            return; 
        }
        
        transform.LookAt(transform.parent.transform.forward);
        //transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private void ManageShooting(Enemy target)
    {
        attackTimer += Time.deltaTime;
        
        if(attackTimer >= attackDelay)
        {
            attackTimer = 0f;
            Shoot(target);
        } 
    }

    private void Shoot(Enemy target)
    {
        int damage = GetDamage(out bool isCritical);
        
        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, (target.transform.position - transform.position).normalized, isCritical);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        
        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100f);
        
        criticalChance = (int)(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100f));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);

        range += playerStatsManager.GetStatValue(Stat.Range) / 10f;   //dividing by 10 is a design choice .. we can change it
    }
}
