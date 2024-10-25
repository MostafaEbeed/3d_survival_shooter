using System;
using Kawaii_Survivor.Scripts.Enemy;
using UnityEngine;


[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rb;
    private Collider2D collider;
    private RangeWeapon rangeWeapon;
    
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private int damage;
    private Enemy enemyTarget;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }
    
    public void Shoot(int damage, Vector2 direction)
    {
        Invoke("Release", 1f);
        
        this.damage = damage;
        
        transform.right = direction;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
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
        enemy.TakeDamage(damage);
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
