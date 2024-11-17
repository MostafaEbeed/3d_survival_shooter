using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    private int requiredXp;
    private int currentXp;
    private int level;
    private int levelsEarnedThisWave;
    
    [Header("Visuals")] 
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("DEBUG")] 
    [SerializeField] private bool DEBUG;
    
    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallback;

        
    }
    
    private void Start()
    {
        xpBar = UIManager.instance.XPBar;
        levelText = UIManager.instance.XPText;
        
        UpdateRequiredXp();
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
    }

    private void UpdateRequiredXp()
    {
        requiredXp = (level + 1) * 5;
    }
    
    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp / requiredXp;
        levelText.text = "lvl " + (level + 1);
    }
    
    private void CandyCollectedCallback(Candy candy)
    {
        currentXp++;

        if (currentXp >= requiredXp)
            LevelUp();
        
        UpdateVisuals();
    }

    private void LevelUp()
    {
        levelsEarnedThisWave++;
        level++;
        currentXp = 0;
        UpdateVisuals();
    }

    public bool HasLeveledUp()
    {
        if(DEBUG)
            return true;
        
        if (levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }
        
        return false;
    }
}