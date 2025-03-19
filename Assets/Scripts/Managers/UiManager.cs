using TMPro;
using UnityEngine;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            gameManager.OnCoinPickUp += SetScoreText;
        }

        private void SetScoreText(int score)
        {
            scoreText.text = score.ToString("D6");
        } 
    }
}
