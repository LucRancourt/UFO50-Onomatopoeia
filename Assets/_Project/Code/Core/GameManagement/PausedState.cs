using UnityEngine;

using _Project.Code.Core.StateMachine;
using _Project.Code.Core.Events;


namespace _Project.Code.Core.GameManagement
{
    public class PausedState : BaseState
    {
        private readonly GameManagementService _gameManagement;
        private InputController _inputService;
        private float _previousTimeScale;

        public PausedState(GameManagementService gameManagement)
        {
            _gameManagement = gameManagement;
        }

        public override void Enter()
        {
            _inputService = ServiceLocator.ServiceLocator.Get<InputController>();
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            _inputService?.EnableUIActions();

            EventBus.Instance.Publish(new GameStateChangedEvent { StateName = "Paused" });
            EventBus.Instance.Publish(new GamePausedEvent());
        }

        public override void Exit()
        {
            Time.timeScale = _previousTimeScale;
            EventBus.Instance.Publish(new GameResumedEvent());
        }
    }
}
