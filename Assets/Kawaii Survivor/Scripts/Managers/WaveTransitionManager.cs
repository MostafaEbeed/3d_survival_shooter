using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager instance;
    
    [FormerlySerializedAs("upgradesButtons")]
    
    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;
    
    [Header("Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private GameObject upgradesContainersParent;
    [SerializeField] private UpgradeContainer[] upgradesContainers;

    [Header("Chest Related Stuff")] 
    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    [Header("Settings")] 
    private int chestsCollected;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        
        Chest.onCollected += ChestCollectedCallback;
    }

    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }
    
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        if (chestsCollected > 0)
        {
            Debug.Log("Number Of Chests Collected " + chestsCollected);
            ShowObject();
        }
        else
        {
            ConfigureUpgradeContainers();
        }
    }

    private void ShowObject()
    {
        chestsCollected--;

        upgradesContainersParent.SetActive(false);
        chestContainerParent.gameObject.SetActive(true);
        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObject = objectDatas[Random.Range(0, objectDatas.Length)];
        
        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObject);
        
        containerInstance.TakeButton.onClick.AddListener(()=> TakeButtonCallback(randomObject));
        containerInstance.RecycleButton.onClick.AddListener(()=> RecycleButtonCallback(randomObject));
    }

    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
        
        TryOpenChest();
    }

    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice); 
        TryOpenChest();
    }
    
    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradesContainersParent.SetActive(true);
        chestContainerParent.gameObject.SetActive(false);
        for (int i = 0; i < upgradesContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            Sprite upgradeSprite = ResourcesManager.GetStatIcon(stat);
            
            string randomStateString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);
            
            upgradesContainers[i].Configure(upgradeSprite, randomStateString, buttonString);
            
            upgradesContainers[i].Button.onClick.RemoveAllListeners();

            upgradesContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradesContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }
    }

    private void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value = 0f;
        
        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + "x";
                break;
            
            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value;
                break;
            
            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString("F2");
                break;
            
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.Luck:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            
            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
        }

        //buttonString = Enums.FormatStatName(stat) + "\n" + buttonString;
        
        return () => playerStatsManager.AddPLayerStat(stat, value);
    }
    
    private void ChestCollectedCallback()
    {
        chestsCollected++;
    }

    public bool HasCollectedChest()
    {
        return chestsCollected > 0;
    }
}
