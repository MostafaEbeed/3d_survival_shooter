using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    private Player player;
    [SerializeField] private Transform shootingPoint;

    [Header("Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [Header("Bullet Pooling")]
    [SerializeField] private EnemyBullet bulletPrefab;
    private ObjectPool<EnemyBullet> bulletsPool;

    void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;

        bulletsPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    void Update()
    {
        
    }

    private EnemyBullet CreateFunction()
    {
        EnemyBullet bulletInstance = (EnemyBullet)Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);

        return bulletInstance;
    }

    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletsPool.Release(bullet);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    public void AutoAim()
    {
        ManageShhoting();
    }

    private void ManageShhoting()
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
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;

        EnemyBullet bulletInstance = bulletsPool.Get();
        bulletInstance.Shoot(damage, direction);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + gizmosDirection * 5);
    } */
}
