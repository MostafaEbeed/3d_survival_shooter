using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    
    public TextMeshProUGUI HealthText => healthText;
}
