using System.Collections;
using _TripleMatch._Scripts.ControllerRelated;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Hop._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        private bool _isGameStarted;
        private UIController _uiController;

        //SCORE RELATED
        private int _currentScore, _highScore;
        private const string HIGHSCORE = "HighScore";

        public int CurrentScore => _currentScore;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //Vibration.Init();
            _uiController = UIController.instance;
            _highScore = PlayerPrefs.GetInt(HIGHSCORE, 0);
            _uiController.UpdateHighScoreTextUi(_highScore);
        }

        private void StartGame()
        {
            if (_isGameStarted) return;
            _isGameStarted = true;
            MainController.instance.SetActionType(GameState.Gameplay);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && MainController.instance.GameState == GameState.Create)
            {
                StartGame();
            }
            if (Input.GetMouseButtonDown(1)) On_RetryButtonClicked();
        }

        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }

        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Levelfail)
            {
                //Debug.Log("LEVEL FAIL...");
                SaveHighScore();
            }
        }

        void SaveHighScore()
        {
            if(_currentScore > _highScore)
                PlayerPrefs.SetInt(HIGHSCORE, _currentScore);
        }

        public void SetCurrentScore()
        {
            _currentScore++;
            _uiController.UpdateScoreTextUi(_currentScore);
        }
        

        public void On_RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public IEnumerator DumpUnUsed(GameObject go, float dur)
        {
            yield return new WaitForSeconds(dur);
            go.SetActive(false);
        }
    }
}