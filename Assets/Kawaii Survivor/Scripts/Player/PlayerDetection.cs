using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [FormerlySerializedAs("daveCollider")]
    [Header("Colliders")]
    [SerializeField] private CircleCollider2D collectablesCollider;


    /*private void FixedUpdate()
    {
        Collider2D[] candyColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + daveCollider.offset, daveCollider.radius);

        foreach (Collider2D candyCollider in candyColliders)
        {
            if (candyCollider.TryGetComponent(out Candy candy))
            {
                Destroy(candy.gameObject);
            } 
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            if (!other.IsTouching(collectablesCollider))
            {
                return;
            }
            
            collectable.Collect(GetComponent<Player>());
        }
    }
}
