using _Hop._Scripts.ControllerRelated;
using TMPro;
using UnityEngine;

namespace _Hop._Scripts.ViewRelated
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        void OnEnable()
        {
            scoreText.text = "SCORE: " + GameController.instance.CurrentScore;
        }
    }
}
