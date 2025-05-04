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
    /// User interface logic of the settings scene.
    /// </summary>
    public class SettingsUI : UserInterface
    {
        [SerializeField] private Button returnButton = null;

        private void Start()
        {
            this.returnButton.onClick.AddListener(OnBackButtonClick);
        }

        /// <summary>
        /// Returns to the settings scene
        /// </summary>
        protected override void OnBackButtonClick()
        {
            
        }
        
    }
}