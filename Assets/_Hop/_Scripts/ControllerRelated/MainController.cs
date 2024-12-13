using System;
using UnityEngine;

namespace _Hop._Scripts.ControllerRelated
{
    public enum GameState
    {
        None,
        Create,
        Gameplay,
        Levelwin,
        Levelfail
    }

    public class MainController : MonoBehaviour
    {
        public static MainController instance;

        [SerializeField] private GameState _gameState;
        public static event Action<GameState, GameState> GameStateChanged;
        public static event Action EnemyShotEvent;

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;
                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CreateGame();
        }

        void CreateGame()
        {
            GameState = GameState.Create;
        }

        public void SetActionType(GameState _curState)
        {
            GameState = _curState;
        }

        public void InvokeEnemyShotEvent()
        {
            EnemyShotEvent?.Invoke();
            Debug.Log("Shot Event !");
        }
    }
}