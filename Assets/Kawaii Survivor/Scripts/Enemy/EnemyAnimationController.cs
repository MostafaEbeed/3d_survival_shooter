using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyAnimationController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] protected Animator animator;
    [SerializeField] private AnimatorOverrideController animatorController;
    
    private EnemyMovement movement;
    private bool canAnimate = false;
    
    void Start()
    {
        animator.runtimeAnimatorController = animatorController;
        movement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if(canAnimate)
            animator.SetFloat("Speed", movement.Agent.speed);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void StartAnimations()
    {
        canAnimate = true;
    }
}
