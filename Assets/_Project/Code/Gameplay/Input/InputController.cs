using UnityEngine;
using UnityEngine.InputSystem;
using System;

using _Project.Code.Core.Events;
using _Project.Code.Core.ServiceLocator;


public class InputController : MonoBehaviourService
{
    private PlayerInputActions _inputActions;

    #region Actions
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    #endregion


    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    public void OnEnable()
    {
        _inputActions.Gameplay.Move.performed += HandleMovePerformed;
        _inputActions.Gameplay.Move.canceled += HandleMoveCanceled;

        _inputActions.Gameplay.Look.performed += HandleLookPerformed;

        _inputActions.Gameplay.PauseGame.performed += HandlePausePerformed;

        _inputActions.Gameplay.Enable();

        _inputActions.UI.UnPause.performed += HandlePausePerformed;

    }

    #region Handlers
    private void HandleMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandleMoveCanceled(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(Vector2.zero);
    }

    private void HandleLookPerformed(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandlePausePerformed(InputAction.CallbackContext context)
    {
        EventBus.Instance.Publish(new PauseInputEvent());
    }
    #endregion

    #region Action Enablers/Disablers 
    public void EnableGameplayActions()
    {
        _inputActions.Gameplay.Enable();
        _inputActions.UI.Disable();
    }

    public void EnableUIActions()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.UI.Enable();
    }

    public void DisableAllActions()
    {
        _inputActions.Gameplay.Disable();
        _inputActions.UI.Disable();
    }
    #endregion

    public void OnDisable()
    {
        if (_inputActions != null)
        {
            _inputActions.Gameplay.Disable();

            _inputActions.Gameplay.Move.performed -= HandleMovePerformed;
            _inputActions.Gameplay.Move.canceled -= HandleMoveCanceled;

            _inputActions.Gameplay.Look.performed -= HandleLookPerformed;

            _inputActions.Gameplay.PauseGame.performed -= HandlePausePerformed;

            _inputActions.UI.UnPause.performed -= HandlePausePerformed;
        }
    }
}