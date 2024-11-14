using System;
using NaughtyAttributes;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }

    [Header("Actions")]
    public static Action onUpdated;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Candy.onCollected += AddCandyCurrency;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= AddCandyCurrency;
    }

    private void Start()
    {
        UpdateTexts();
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;

        UpdateTexts();
        
        onUpdated?.Invoke();
    }

    private void AddCandyCurrency(Candy candy)
    {
        Currency += 1;

        UpdateTexts();
        
        onUpdated?.Invoke();
    }
    
    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)     
        {
            text.UpdateText(Currency.ToString());
        }
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }

    public void UseCurrency(int price)
    {
        AddCurrency(-price);
    }

    [Button]
    private void Add500Currency()
    {
        AddCurrency(500);
    }
}