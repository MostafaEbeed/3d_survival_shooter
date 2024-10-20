using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CircleCollider2D collider;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage); 
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + collider.offset;
    }
}
