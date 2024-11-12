using System;
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

    [SerializeField] private Button purchaseButton;
    
    [Header("Color")]
    [SerializeField] private Image[] levelDependentImage;
    [SerializeField] private Outline outLine;
    
    [Header("Lock Elements")]
    [SerializeField] private Image LockImage;
    [SerializeField] private Sprite LockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    [Header("Purchasing")]
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    private int weaponLevel;
    
    [Header("Actions")]
    public static Action<ShopItemContainer, int> onPurchased;
    
    public bool IsLocked { get; private set; }


    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }
    
    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;
        
        weaponLevel = level;
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (lvl {level + 1})";

        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        
        priceText.text = weaponPrice.ToString();
        
        Color imageColor = ColorHolder.Instance.GetColor(level);
        nameText.color = imageColor;
        
        outLine.effectColor = ColorHolder.Instance.GetOutlineColor(level);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

        purchaseButton.onClick.AddListener(() => Purchase());
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(weaponPrice);
    }

    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;
        
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

        ConfigureStatContainers(objectData.BaseStats);
        
        purchaseButton.onClick.AddListener(() => Purchase());
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(objectData.Price);
    }

    private void Purchase()
    {
        onPurchased?.Invoke(this, weaponLevel);
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
    
    private void CurrencyUpdatedCallback()
    {
        int itemPrice;

        if (WeaponData != null)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        else
            itemPrice = ObjectData.Price;
        
       purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice); 
    }
}