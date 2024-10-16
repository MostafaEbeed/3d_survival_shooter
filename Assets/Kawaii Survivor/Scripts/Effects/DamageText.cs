using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;


    [NaughtyAttributes.Button]
    public void Animate(int damage)
    {
        damageText.text = damage.ToString();

        animator.Play("Animate");
    }
}
