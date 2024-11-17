using System;
using Kawaii_Survivor.Scripts.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Settings")]
    [SerializeField] private int baseMaxHealth = 10;
    private float maxHealth;
    private float health;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private HighLightShadingEffect highLight;
    
    [Header("Actiond")] 
    public static Action<Vector2> onAttackeDodged;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void Start()
    {
        healthSlider = UIManager.instance.HealthBar;
        healthText = UIManager.instance.HealthText;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    private void Update()
    {
        if (health < maxHealth)
            RecoverHealth();
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;
            
            float healthToAdd = Mathf.Min(0.1f, maxHealth - health);
            health += healthToAdd;
            UpdateUI();
        }
    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if(health >= maxHealth)
            return;

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Mathf.Min(lifeStealValue, maxHealth - health);
        
        health += healthToAdd;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackeDodged?.Invoke(transform.position);
            return;
        }
        
        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;
        highLight.FlashOnImapct();
        UpdateUI();

        if (health <= 0)
            PassAway();
    }

    private bool ShouldDodge()
    {
        return Random.Range(0f, 101f) < dodge;
    }

    private void PassAway()
    {
        Debug.Log("Game Overrrrrrr");
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateUI()
    {
        float healthBarValue = health / maxHealth;
        if (healthSlider)
        {
            healthSlider.value = healthBarValue;
            healthText.text = (int)health + " / " + maxHealth;
        }
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);
        
        
        health = maxHealth;
        UpdateUI();
        
        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = Mathf.Max(0.0001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}
