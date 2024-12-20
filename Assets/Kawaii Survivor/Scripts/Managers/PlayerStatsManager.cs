using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private CharacterDataSO playerData;
    
    [Header("Settings")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> objectAddends = new Dictionary<Stat, float>();

    private void Awake()
    {
        playerStats = playerData.BaseStats;

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0);
            objectAddends.Add(kvp.Key, 0);
        }
    }

    private void Start()
    {
        UpdatePlayerStats();
    }


    public void AddPLayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
        {
            addends[stat] += value;
        }
        else
        {
            Debug.LogError($"The key {stat} is not present in the player stats manager");
        }

        UpdatePlayerStats();
    }

    public void AddObject(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            objectAddends[kvp.Key] += kvp.Value;
        }
        
        UpdatePlayerStats();
    }
    
    public void RemoveObjectStats(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            objectAddends[kvp.Key] -= kvp.Value;
        }
        
        UpdatePlayerStats();
    }
    
    public float GetStatValue(Stat stat)
    {
        return playerStats[stat] + addends[stat] + objectAddends[stat];
    }
    
    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IPlayerStatsDependency>();

        foreach (IPlayerStatsDependency playerStatsDependency in playerStatsDependencies)
        {
            playerStatsDependency.UpdateStats(this);
        }
    }
}