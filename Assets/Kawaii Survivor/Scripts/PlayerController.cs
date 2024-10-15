using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick playerJoystick;
    private Rigidbody2D rig;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.linearVelocity = Vector2.right;
    }

    
    private void FixedUpdate()
    {
        rig.linearVelocity = playerJoystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }
}
