using System;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody rb;
    private Collider collider;
    private RangeWeapon rangeWeapon;
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private int damage;
    private bool isCriticalHit;
    private Enemy enemyTarget;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }
    
    public void Shoot(int damage, Vector3 direction, bool isCriticalHit)
    {
        Invoke("Release", 1f);
        
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;
        
        transform.forward = direction;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enemyTarget != null)
            return;
        
        if (IsInLayerMask(other.gameObject.layer, enemyMask))
        {
            enemyTarget = other.GetComponent<Enemy>();
            
            CancelInvoke();
            
            Attack(enemyTarget);
            Release();
        }
    }

    private void Release()
    {
        if(!gameObject.activeSelf)
            return;
        
        rangeWeapon.ReleaseBullet(this);
    }
    
    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
    
    public void Reload()
    {
        enemyTarget = null;
        
        rb.linearVelocity = Vector2.zero;
        this.collider.enabled = true;
    }
}
