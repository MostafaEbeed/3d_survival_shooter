using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Refs")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;
    
    [Header("Elements")]
    [SerializeField] private Transform inventoryItemParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo inventoryItemInfo;

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergeCallback;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergeCallback;
    }
    
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
        }
    }

    private void Configure()
    {
        inventoryItemParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null)
                continue;
            
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemParent);
            container.Configure(weapons[i], i,()=> ShowItemInfo(container));
        }
        
        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemParent);
            container.Configure(objectDatas[i], ()=> ShowItemInfo(container));
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else
            ShowObjectInfo(container.ObjectData);
    }
    
    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        inventoryItemInfo.Configure(weapon);
        
        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));
        
        shopManagerUI.ShowItemInfoPanel();
    }

    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);
        
        Configure();
        
        shopManagerUI.HideItemInfoPanel();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        inventoryItemInfo.Configure(objectData);
        
        inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));
        
        shopManagerUI.ShowItemInfoPanel();
    }

    private void RecycleObject(ObjectDataSO objectToRecycle)
    {
        playerObjects.RecycleObject(objectToRecycle); 
        
        Configure();
        
        shopManagerUI.HideItemInfoPanel();
    }
    
    private void ItemPurchasedCallback()
    {
        Configure();
    }
    
    private void WeaponMergeCallback(Weapon mergedWeapon)
    {
        Configure();
        
        inventoryItemInfo.Configure(mergedWeapon);
    }
}