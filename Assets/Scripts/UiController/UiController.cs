using System.Collections.Generic;
using System.Linq;
using PlayerController.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UiController
{
    public class UiController : MonoBehaviour
    {
        [SerializeField]private string quitLabelTemplate = "HOLD ESC FOR {0} SECONDS TO QUIT";

        [SerializeField] private GameObject playerIcon;
        [SerializeField] private Transform livesContainer;
        [SerializeField] private Text playerScore;

        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject endGameScreen;
        [SerializeField] private Text endGameScreenText;
        [SerializeField] private GameObject quitLabel;
        [SerializeField] private Text quitLabelText;

        private List<GameObject> _playerLives = new List<GameObject>();

        public void Initialize(ScoreCounter scoreCounter, IPlayerController playerController, uint secondsToQuit)
        {
            playerScore.gameObject.SetActive(true);

            scoreCounter.OnScoreChange += UpdateScore;
            playerController.OnLivesChange += UpdateLives;

            quitLabelText.text = string.Format(quitLabelTemplate, secondsToQuit);
            quitLabel.SetActive(true);

            UpdateLives(playerController.GetMaxLives());
            UpdateScore(0);
        }

        private void UpdateScore(uint score)
        {
            playerScore.text = score.ToString();
        }

        //TODO: add pool
        private void UpdateLives(uint currentLives)
        {
            if (currentLives == _playerLives.Count)
            {
                return;
            }

            while (_playerLives.Count > currentLives)
            {
                var lifeIcon = _playerLives.LastOrDefault();
                _playerLives.Remove(lifeIcon);
                Destroy(lifeIcon);
            }

            while (_playerLives.Count < currentLives)
            {
                var lifeIcon = Instantiate(playerIcon, livesContainer);
                lifeIcon.SetActive(true);
                _playerLives.Add(lifeIcon);
            }
        }

        public void Reset()
        {
            UpdateLives(0);
            UpdateScore(0);
        }

        public void SetScreen(Screen screen, bool status)
        {
            switch (screen)
            {
                case Screen.Start:
                    playerScore.gameObject.SetActive(!status);
                    startScreen.SetActive(status);
                    break;
                case Screen.End:
                    endGameScreen.SetActive(status);
                    playerScore.gameObject.SetActive(!status);
                    endGameScreenText.text += $" {playerScore.text}";
                    break;
            }
        }
    }

    public enum Screen
    {
        Start,
        End
    }
}