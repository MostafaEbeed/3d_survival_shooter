using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;
    
    [Header("Components")]
    [SerializeField] private Collider collider;
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
    }
    
    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage); 
    }

    public Vector3 GetCenter()
    {
        return transform.position + Vector3.up * 0.5f/*+ collider.transform.localPosition*/;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
