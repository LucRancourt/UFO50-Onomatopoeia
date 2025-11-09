using UnityEngine;

namespace _Project.Code.Core.GameManagement
{
    public class SceneGameState : MonoBehaviour
    {
        [SerializeField] private GameStateType _initialState = GameStateType.Gameplay;

        private void Start()
        {
            var gameManagement = ServiceLocator.ServiceLocator.Get<GameManagementService>();

            switch (_initialState)
            {
                case GameStateType.Gameplay:
                    gameManagement.TransitionToGameplay();
                    break;
                case GameStateType.Menu:
                    gameManagement.TransitionToMenu();
                    break;
                case GameStateType.Paused:
                    gameManagement.TransitionToPaused();
                    break;
            }
        }
    }

    public enum GameStateType
    {
        Gameplay,
        Menu,
        Paused
    }
}
