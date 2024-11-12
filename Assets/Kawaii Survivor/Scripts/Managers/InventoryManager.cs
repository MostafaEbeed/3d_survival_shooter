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
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemParent);
            container.Configure(weapons[i], ()=> ShowItemInfo(container));
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
            ShowWeaponInfo(container.Weapon);
        else
            ShowObjectInfo(container.ObjectData);
    }
    
    private void ShowWeaponInfo(Weapon weapon)
    {
        inventoryItemInfo.Configure(weapon);
        shopManagerUI.ShowItemInfoPanel();
    }
    
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        inventoryItemInfo.Configure(objectData);
        shopManagerUI.ShowItemInfoPanel();
    }
}