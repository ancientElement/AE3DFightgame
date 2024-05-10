using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AEActionSystem
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")] public Vector2 move;
        private ActionSystem ActionSystem;
        public Vector2 look;

        // public bool jump;
        public bool sprint;

        [Header("Movement Settings")] public bool analogMovement;

        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private void Start()
        {
            ActionSystem = GetComponent<ActionSystem>();
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputAction.CallbackContext ctx)
        {
            // Debug.Log(ctx.ReadValue<Vector2>());
            MoveInput(ctx.ReadValue<Vector2>());
        }

        // public void OnMove(InputValue value)
        // {
        //     MoveInput(value.Get<Vector2>());
        // }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            if (cursorInputForLook)
            {
                LookInput(ctx.ReadValue<Vector2>());
            }
        }

        // public void OnLook(InputValue value)
        // {
        //     if (cursorInputForLook)
        //     {
        //         LookInput(value.Get<Vector2>());
        //     }
        // }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintInput(ctx.action.IsPressed());
        }

        // public void OnSprint(InputValue value)
        // {
        //     SprintInput(value.isPressed);
        // }

        public void ActionKeyTriger(InputAction.CallbackContext ctx)
        {
            if (ActionSystem.CanBeInterrupt && ctx.action.triggered)
            {
                UnityEngine.Debug.Log(ctx.action.name);
                ActionSystem.ProcessInput(ctx.action.name);
            }
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}