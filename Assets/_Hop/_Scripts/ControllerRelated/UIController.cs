using System.Collections.Generic;
using _Hop._Scripts.ControllerRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _TripleMatch._Scripts.ControllerRelated
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;
        public TextMeshProUGUI scoreText, highScoreText;
        public CanvasGroup mainMenuGroup;
        public GameObject gameOverScreen;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
        }

        public void UpdateScoreTextUi(int score) => scoreText.text = score.ToString();
        public void UpdateHighScoreTextUi(int score) => highScoreText.text = score.ToString();
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
            if (newState == GameState.Gameplay)
            {
                scoreText.gameObject.SetActive(true);
                mainMenuGroup.DOFade(0, 1).OnComplete(() => { mainMenuGroup.gameObject.SetActive(false);});
            }

            if (newState == GameState.Levelfail)
            {
                scoreText.gameObject.SetActive(false);
                gameOverScreen.SetActive(true);
                gameOverScreen.GetComponent<CanvasGroup>().DOFade(1, 1);
            }
        }
    }   
}
