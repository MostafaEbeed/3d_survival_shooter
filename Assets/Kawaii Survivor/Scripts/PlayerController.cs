using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.linearVelocity = Vector2.right;
    }

    
    void Update()
    {
        
    }
}
