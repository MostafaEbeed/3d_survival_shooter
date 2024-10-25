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

        Vector2 targetUpVector = Vector3.up;

        if(closestEnemy)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            
            ManageShooting();
            return; 
        }
        
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        
        if(attackTimer >= attackDelay)
        {
            attackTimer = 0f;
            Shoot();
        } 
    }

    private void Shoot()
    {
        int damage = GetDamage(out bool isCritical);
        
        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, transform.up, isCritical);
    }
}
