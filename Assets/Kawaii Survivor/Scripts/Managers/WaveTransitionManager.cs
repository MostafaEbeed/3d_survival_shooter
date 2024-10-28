using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [FormerlySerializedAs("upgradesButtons")]
    [Header("Elements")]
    [SerializeField] private UpgradeContainer[] upgradesContainers;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers();
                break;
        }
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradesContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            
            string randomStateString = Enums.FormatStatName(stat);
            
            upgradesContainers[i].Configure(null, randomStateString, Random.Range(0,110).ToString());
            
            upgradesContainers[i].Button.onClick.RemoveAllListeners();
            upgradesContainers[i].Button.onClick.AddListener(() => Debug.Log(randomStateString));
        }
    }
}
