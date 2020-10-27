using System.Collections.Generic;
using System.Linq;
using PlayerController.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UiController
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject playerIcon;
        [SerializeField] private Transform livesContainer;
        [SerializeField] private Text playerScore;

        private List<GameObject> _playerLives = new List<GameObject>();

        public void Initialize(ScoreCounter scoreCounter, IPlayerController playerController)
        {
            UpdateLives(playerController.GetMaxLives());
            UpdateScore(0);

            scoreCounter.OnScoreChange += UpdateScore;
            playerController.OnLivesChange += UpdateLives;
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
    }
}