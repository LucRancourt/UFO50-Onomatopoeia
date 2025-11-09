using UnityEngine;
using _Project.Code.Core.StateMachine;
using _Project.Code.Core.Events;

namespace _Project.Code.Core.GameManagement
{
    public class MenuState : BaseState
    {
        private readonly GameManagementService _gameManagement;
        private InputController _inputService;

        public MenuState(GameManagementService gameManagement)
        {
            _gameManagement = gameManagement;
        }

        public override void Enter()
        {
            _inputService = ServiceLocator.ServiceLocator.Get<InputController>();
            Time.timeScale = 1f;
            _inputService?.EnableUIActions();

            EventBus.Instance.Publish(new GameStateChangedEvent { StateName = "Menu" });
        }
    }
}
