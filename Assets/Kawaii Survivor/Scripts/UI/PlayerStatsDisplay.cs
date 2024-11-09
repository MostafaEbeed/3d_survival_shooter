using System;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private Transform playerStatContainerParent;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;
        
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = playerStatContainerParent.GetChild(index).GetComponent<StatContainer>();
            statContainer.gameObject.SetActive(true);
            
            Sprite statIcon = ResourcesManager.GetStatIcon(stat);

            float statValue = playerStatsManager.GetStatValue(stat);
            
            statContainer.Configure(statIcon, Enums.FormatStatName(stat), statValue);
            
            index++;
        }

        for (int i = index; i < playerStatContainerParent.childCount; i++)
        {
           playerStatContainerParent.GetChild(i).gameObject.SetActive(false); 
        }
    }
}
