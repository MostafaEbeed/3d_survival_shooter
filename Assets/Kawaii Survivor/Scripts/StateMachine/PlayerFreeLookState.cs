using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.22f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFade(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        //CheckForEnemies();

         /*if (stateMachine.InputReader.MovementValue == Vector2.zero)
         {
             stateMachine.Animator.SetFloat(FreeLookSpeedHash, movement.magnitude, AnimatorDampTime, deltaTime);
             return;
         } */

        //for mobile controls use the up chunk of code instead for PC
        if (stateMachine.mobileJoystick.GetMoveVector() == Vector3.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, movement.magnitude, AnimatorDampTime, deltaTime);
            return;
        } 

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, movement.magnitude, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //Debug.Log(stateMachine.InputReader.MovementValue.x + " " + stateMachine.InputReader.MovementValue.y);
        //SetStickDirection(new Vector3(stateMachine.InputReader.MovementValue.x, 0, stateMachine.InputReader.MovementValue.y));
        Vector3 startPoint = new Vector3(stateMachine.transform.position.x, 0.5f, stateMachine.transform.position.z);
        Debug.DrawRay(startPoint, stickDirection * 10, Color.blue);

        //Debug.Log(stateMachine.mobileJoystick.GetMoveVector());
        //Debug.Log(stateMachine.InputReader.MovementValue);

        //return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;

        //for mobile controls
        Vector3 moveTarget = new Vector3(right.x * stateMachine.mobileJoystick.GetMoveVector().x, 0.0f, forward.z * stateMachine.mobileJoystick.GetMoveVector().y);
        //return forward * stateMachine.mobileJoystick.GetMoveVector().y + right * stateMachine.mobileJoystick.GetMoveVector().x;
        return moveTarget.normalized;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

    private void CheckForEnemies()
    {
        Ray ray = new Ray(stateMachine.transform.position, stickDirection.normalized * 10f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
        }
    }
}
