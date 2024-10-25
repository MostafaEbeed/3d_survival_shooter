using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;


    [NaughtyAttributes.Button]
    public void Animate(int damage, bool isCriticalHit)
    {
        damageText.text = damage.ToString();
        damageText.color  = isCriticalHit ? Color.yellow : Color.white;
        
        animator.Play("Animate");
    }
}
