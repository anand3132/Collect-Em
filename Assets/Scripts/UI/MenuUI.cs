using RedGaintGames.CollectEM.Core;
using RedGaintGames.CollectEM.Core.UI;

namespace RedGaintGames.CollectEM.Game
{
    using CollectEM;
    using CollectEM.Core;
    using CollectEM.Core.UI;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// User interface logic of the menu scene.
    /// </summary>
    public class MenuUI : UserInterface
    {
        //Buttons
        [SerializeField] private Button PlayButton = null;
        [SerializeField] private Button HelpButton = null;
        [SerializeField] private Button SettingsButton = null;

        //Highscore text
        [SerializeField] private Text HighscoreText = null;

        private void Start()
        {
            //Listen to button click events
            this.PlayButton.onClick.AddListener(LoadGameScene);
            this.HelpButton.onClick.AddListener(LoadHelpScene);
            this.SettingsButton.onClick.AddListener(LoadSettingsScene);

            //Set highscore text
            this.HighscoreText.text = ScoreManager.Instance.Highscore.ToString();
        }

        /// <summary>
        /// Loads the game scene
        /// </summary>
        private void LoadGameScene()
        {
        }

        /// <summary>
        /// Loads the help scene
        /// </summary>
        private void LoadHelpScene()
        {
        }

        /// <summary>
        /// Loads the settings scene
        /// </summary>
        private void LoadSettingsScene()
        {
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        protected override void OnBackButtonClick()
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            PlayerPrefs.Save();

            Application.Quit();
        }
    }
}