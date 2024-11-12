using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image container;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    
    public void Configure(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        icon.sprite = itemIcon;
    }

    public void Configure(Weapon weapon, Action clickedCallback)
    {
        Weapon = weapon;
        
        Color color = ColorHolder.Instance.GetColor(weapon.Level);
        Sprite icon = weapon.weaponData.Sprite;
        
        Configure(color, icon);
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }
    
    public void Configure(ObjectDataSO objectData, Action clickedCallback)
    {
        ObjectData = objectData;
        
        Color color = ColorHolder.Instance.GetColor(objectData.Rarity);
        Sprite icon = objectData.icon;
        
        Configure(color, icon);
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
    }
}
