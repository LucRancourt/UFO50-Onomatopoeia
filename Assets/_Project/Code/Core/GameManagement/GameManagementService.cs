using _Project.Code.Core.ServiceLocator;
using _Project.Code.Core.StateMachine;
using _Project.Code.Core.Events;


namespace _Project.Code.Core.GameManagement
{
    public class GameManagementService : MonoBehaviourService, IEventSubscriber
    {
        private StateMachine<IState> _stateMachine;

        public IState CurrentState => _stateMachine?.CurrentState;
        
        public override void Initialize()
        {
            var gameplayState = new MenuState(this);
            _stateMachine = new StateMachine<IState>(gameplayState);

            _stateMachine.AddState(new PausedState(this));
            _stateMachine.AddState(new GameplayState(this));

            this.Subscribe<PauseInputEvent>(HandlePauseInput);
        }

        private void HandlePauseInput(PauseInputEvent evt)
        {
            if (_stateMachine.CurrentState is GameplayState)
            {
                TransitionToPaused();
            }
            else if (_stateMachine.CurrentState is PausedState)
            {
                TransitionToGameplay();
            }
        }

        public void TransitionToGameplay() => _stateMachine.TransitionTo<GameplayState>();
        public void TransitionToPaused() => _stateMachine.TransitionTo<PausedState>();
        public void TransitionToMenu() => _stateMachine.TransitionTo<MenuState>();

        private void Update()
        {
            _stateMachine?.Update();
        }

        public override void Dispose()
        {
            ClearEventSubscriberOnDisables();
        }

        public void ClearEventSubscriberOnDisables()
        {
            this.OnDestroyEventSubscriber();
        }
    }
}