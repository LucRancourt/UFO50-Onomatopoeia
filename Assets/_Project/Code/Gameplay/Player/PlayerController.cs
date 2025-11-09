using UnityEngine;

using _Project.Code.Core.ServiceLocator;


public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }


    #region InputController - OnEnable/OnDisable
    private void OnEnable()
    {
        if (ServiceLocator.TryGet(out InputController inputController))
        {
            inputController.MoveEvent += HandleMoveInput;
            inputController.LookEvent += HandleLookInput;
        }
        else
        {
            Debug.LogError("ServiceLocator did not have the InputController!");
        }
    }

    private void OnDisable()
    {
        if (ServiceLocator.TryGet(out InputController inputController))
        {
            inputController.MoveEvent -= HandleMoveInput;
            inputController.LookEvent -= HandleLookInput;
        }
        else
        {
            Debug.LogError("ServiceLocator did not have the InputController!");
        }
    }
    #endregion

    #region HandleInputs
    private void HandleMoveInput(Vector2 movement)
    {
        MoveInput = movement;
    }

    private void HandleLookInput(Vector2 look)
    {
        LookInput = look;
    }
    #endregion
}