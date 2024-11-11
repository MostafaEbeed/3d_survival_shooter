using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    
    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;
    
    [field: SerializeField] public Button PurchaseButton { get; private set; }
    
    [Header("Color")]
    [SerializeField] private Image[] levelDependentImage;
    [SerializeField] private Outline outLine;
    
    [Header("Lock Elements")]
    [SerializeField] private Image LockImage;
    [SerializeField] private Sprite LockedSprite;
    [SerializeField] private Sprite unlockedSprite;
    public bool IsLocked { get; private set; }
    
    public void Configure(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (lvl {level + 1})";
        priceText.text = WeaponStatsCalculator.GetPurchasePrice(weaponData, level).ToString();
        
        Color imageColor = ColorHolder.Instance.GetColor(level);
        nameText.color = imageColor;
        
        outLine.effectColor = ColorHolder.Instance.GetOutlineColor(level);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }
    
    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.icon;
        nameText.text = objectData.Name;
        priceText.text = objectData.Price.ToString();
        
        Color imageColor = ColorHolder.Instance.GetColor(objectData.Rarity);
        nameText.color = imageColor;
        
        outLine.effectColor = ColorHolder.Instance.GetOutlineColor(objectData.Rarity);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        }

        //Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        statsContainerParent.Clear();
        StatContainerManager.instance.GenerateContainers(stats, statsContainerParent);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        LockImage.sprite = IsLocked ? LockedSprite : unlockedSprite;
    }
}
