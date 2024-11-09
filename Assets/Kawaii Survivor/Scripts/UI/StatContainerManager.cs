using System;
using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;
    
    [Header("Elements")]
    [SerializeField] private StatContainer statContainer;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();
        
        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer containerInstance = Instantiate(statContainer, parent);
            statContainers.Add(containerInstance);
            
            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            float statValue = kvp.Value;
            
            containerInstance.Configure(icon, statName, statValue);
        }
        
        LeanTween.delayedCall(Time.deltaTime * 2f, ()=> ResizeTexts(statContainers));
    }

    private void ResizeTexts(List<StatContainer> statContainers)
    {
        float minFontSize = 5000f;

        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();
            
            if(fontSize < minFontSize)
                minFontSize = fontSize;
        }

        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize);
        }
    }
}
