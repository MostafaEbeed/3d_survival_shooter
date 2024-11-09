using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;
    
    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }
    
    [SerializeField] public TextMeshProUGUI recyclePriceText;
    
    [Header("Color")]
    [SerializeField] private Image[] levelDependentImage;
    [SerializeField] private Outline outLine;
    
    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.icon;
        nameText.text = objectData.Name;
        recyclePriceText.text = objectData.RecyclePrice.ToString();
        
        Color imageColor = ColorHolder.Instance.GetColor(objectData.Rarity);
        nameText.color = imageColor;
        
        outLine.effectColor = ColorHolder.Instance.GetOutlineColor(objectData.Rarity);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        }

        ConfigureStatContainers(objectData.BaseStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.instance.GenerateContainers(stats, statsContainerParent);
    }

}
