using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [field: SerializeField] public Button Button { get; private set; }

    
    [Header("Color")]
    [SerializeField] private Image[] levelDependentImage;
    
    
    public void Configure(Sprite sprite, string name, int level)
    {
        icon.sprite = sprite;
        nameText.text = name;

        Color imageColor = ColorHolder.Instance.GetColor(level);

        foreach (Image image in levelDependentImage)
        {
            image.color = imageColor;
        } 
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