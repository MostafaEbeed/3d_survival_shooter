using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header("Colors")] 
    [SerializeField] private Image container;
    
    [Header("Stats")] 
    [SerializeField] private Transform statsParent;

    public void Configure(Weapon weapon)
    {
        Configure(weapon.weaponData.Sprite
                , weapon.weaponData.Name + "(lvl " + (weapon.Level + 1) + ")"
                , ColorHolder.Instance.GetColor(weapon.Level)
                , WeaponStatsCalculator.GetRecylePrice(weapon.weaponData, weapon.Level)
                , WeaponStatsCalculator.GetStats(weapon.weaponData, weapon.Level));
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure(objectData.icon
            , objectData.Name
            , ColorHolder.Instance.GetColor(objectData.Rarity)
            , objectData.RecyclePrice
            , objectData.BaseStats);
    }
    
    public void Configure(Sprite itemIcon, string name, Color containerColor, int recyclePrice, Dictionary<Stat, float> stats)
    {
        icon.sprite = itemIcon;
        itemNameText.text = name;
        itemNameText.color = containerColor;
        
        recyclePriceText.text = recyclePrice.ToString();
        
        container.color = containerColor;
        
        StatContainerManager.instance.GenerateContainers(stats, statsParent);
    }
}
