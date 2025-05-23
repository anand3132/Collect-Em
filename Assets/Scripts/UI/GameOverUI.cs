﻿using RedGaintGames.CollectEM.Core;
using RedGaintGames.CollectEM.Core.UI;

namespace RedGaintGames.CollectEM.Game
{
    using CollectEM;
    using CollectEM.Core;
    using CollectEM.Core.UI;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// User interface logic of the game over scene.
    /// </summary>
    public class GameOverUI : UserInterface
    {
        //Buttons
        [SerializeField] private Button restartButton = null;
        [SerializeField] private Button returnButton = null;

        //UI-Text for the reached score
        [SerializeField] private Text scoreText = null;

        //Highscore Message
        [SerializeField] private GameObject highscoreMessage = null;

        private void Start()
        {
            //Listen to button click events
            this.restartButton.onClick.AddListener(OnRestartButtonClick);
            this.returnButton.onClick.AddListener(OnBackButtonClick);

            //Get the reached score and show it
            this.scoreText.text = GameManager.Score.ToString();

            //Check if the player has reached a new highscore
            if (GameManager.Score > ScoreManager.Instance.Highscore)
            {
                ScoreManager.Instance.Highscore = GameManager.Score;

                //Show highscore message
                this.highscoreMessage.SetActive(true);
            }
        }

        /// <summary>
        /// Restarts the game
        /// </summary>
        private void OnRestartButtonClick()
        {
            
        }

        /// <summary>
        /// Returns to the menu scene
        /// </summary>
        protected override void OnBackButtonClick()
        {
        }
    }
}