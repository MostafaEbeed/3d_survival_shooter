using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    [field: SerializeField] public Vector3 stickDirection { get; private set; }

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion) * deltaTime);
    }

    protected void FaceTarget()
    {
        
    }

    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    protected void FaceDirection(Vector3 direction)
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected void SetStickDirection(Vector3 _value)
    {
        stickDirection = _value;
    }
}
