using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Configure(Sprite icon, string statName, float statValue)
    {
        statImage.sprite = icon;
        statText.text = statName;
        
        float sign = Mathf.Sign(statValue);

        if (statValue == 0)
            sign = 0;
        
        float absStatValue = Mathf.Abs(statValue);
        
        Color statValueTextColor = Color.white;
        
        if(sign > 0)
            statValueTextColor = Color.green;
        else if(sign < 0)
            statValueTextColor = Color.red;

        statValueText.color = statValueTextColor;
        statValueText.text = absStatValue.ToString("F2");
    }

    public float GetFontSize()
    {
        return statText.fontSize;
    }

    public void SetFontSize(float minFontSize)
    {
        statText.fontSizeMax = minFontSize;
        statValueText.fontSizeMax = minFontSize;
    }
}
