using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;
    
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;
    
    [Header("Reroll")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private TextMeshProUGUI rerollPriceText;

    [Header("Actions")]
    public static Action onItemPurchased;
    
    private void Awake()
    {
        ShopItemContainer.onPurchased += ItemPurchasedCallback;
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }
    
    private void OnDestroy()
    {
        ShopItemContainer.onPurchased -= ItemPurchasedCallback;
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
            UpdateRerollVisuals();
        }
    }

    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < containersParent.childCount; i++)
        {
            ShopItemContainer container = containersParent.GetChild(i).GetComponent<ShopItemContainer>();
            
            if(!container.IsLocked)
                toDestroy.Add(container.gameObject);
        }

        while (toDestroy.Count > 0)
        {
            Transform t = toDestroy[0].transform;
            t.SetParent(null);
            Destroy(t.gameObject);
            toDestroy.RemoveAt(0);
        }
        
        int containersToAdd = 6 - containersParent.childCount;
        int weaponContainersCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainersCount;
        
        for (int i = 0; i < weaponContainersCount; i++)
        {
            ShopItemContainer weaponContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon();
            
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0,2));
        }
        
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);

            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();
            
            objectContainerInstance.Configure(randomObject);
        }
    }

    public void Reroll()
    {
        Configure();
        CurrencyManager.instance.UseCurrency(rerollPrice);
    }

    private void UpdateRerollVisuals()
    {
        rerollPriceText.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.instance.HasEnoughCurrency(rerollPrice);
    }
    
    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisuals();
    }
    
    private void ItemPurchasedCallback(ShopItemContainer container, int weaponLevel)
    {
        if (container.WeaponData != null)
        {
            TryPurchaseWeapon(container, weaponLevel);
        }
        else
        {
            PurchaseObject(container);
        }
    }

    private void TryPurchaseWeapon(ShopItemContainer container, int weaponLevel)
    {
        if (playerWeapons.TryAddWeapon(container.WeaponData, weaponLevel))
        {
            int price = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.instance.UseCurrency(price);
            
            Destroy(container.gameObject);
        }

        onItemPurchased?.Invoke();
    }
    
    private void PurchaseObject(ShopItemContainer container)
    {
        playerObjects.AddObject(container.ObjectData);
        
        CurrencyManager.instance.UseCurrency(container.ObjectData.Price);
        
        Destroy(container.gameObject);
        
        onItemPurchased?.Invoke();
    }
}
