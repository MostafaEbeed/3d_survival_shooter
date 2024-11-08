using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;
    
    [field: SerializeField] public Button Button { get; private set; }
    
    [Header("Color")]
    [SerializeField] private Image[] levelDependentImage;
    [SerializeField] private Outline outLine;
    
    public void Configure(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $" (lvl {level + 1})";

        Color imageColor = ColorHolder.Instance.GetColor(level);
        nameText.color = imageColor;
        
        outLine.effectColor = ColorHolder.Instance.GetOutlineColor(level);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.instance.GenerateContainers(calculatedStats, statsContainerParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, 0.2f).setEase(LeanTweenType.easeInOutSine);
    }

    public void Deselct()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, 0.2f);
    }
}