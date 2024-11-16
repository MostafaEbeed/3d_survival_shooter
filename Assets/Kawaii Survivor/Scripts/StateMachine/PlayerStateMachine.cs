using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public Animator Animator { get; private set; }
    
    [field: SerializeField] public MobileJoystick mobileJoystick { get; private set; }

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public float RotationDamping { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    private void HandleTakeDamage()
    {
    }

    private void HandleDie()
    {
    }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }
}
