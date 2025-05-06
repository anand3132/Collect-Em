using RedGaintGames.CollectEM.Core;
using RedGaintGames.CollectEM.Game.PowerUps;
using System.Collections;
using System.Collections.Generic;
using RedGaintGames.CollectEM.Game.Designer;
using UnityEngine;
using UnityEngine.UI;

namespace RedGaintGames.CollectEM.Game
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Configuration")]
        [SerializeField] public Grid grid;
        [SerializeField] private int movesAvailable = 20;
        [SerializeField] private bool powerUpsEnabled = false;
        public GridDesignerUI GridDesignerUI;
        public static int Score { get; private set; }
        public int MovesAvailable { get; set; }
        public bool PowerUpsEnabled {
            get => powerUpsEnabled;
            set => powerUpsEnabled = value; 
        }

        private PowerUpSystem powerUpSystem;

        private void OnEnable() => GameEvents.OnElementsDeSpawned.AddListener(OnElementsDespawned);

        private void OnElementsDespawned(int arg0, List<GridElement> arg1)
        {
            
        }

        private void OnDisable() => GameEvents.OnElementsDeSpawned.RemoveListener(OnElementsDespawned);


        private IEnumerator Start()
        {
            InitializeGame();
            yield return StartCoroutine(RunGame());
            yield return StartCoroutine(EndGame());
        }

        private void InitializeGame()
        {
            Score = 0;
            MovesAvailable = movesAvailable;
    
            // Configure your power-up rules
            var powerUpRules = new List<PowerUpRule>
            {
                new PowerUpRule { MinimumElements = 4, PowerUpType = typeof(DoubleRocketPowerUp) },
                new PowerUpRule { MinimumElements = 6, PowerUpType = typeof(BombPowerUp) }
            };
    
            powerUpSystem = new PowerUpSystem();
            powerUpSystem.Initialize(grid, powerUpRules);
    
            grid.GenerateGrid();
        }
        public IEnumerator RunGame()
        {
            while (MovesAvailable > 0)
            {
                yield return HandlePlayerMove();
                yield return HandleChainReactions();
            }
        }

        
        // private IEnumerator HandlePlayerMove()
        // {
        //     yield return grid.WaitForSelection();
        //
        //     var selectedElements = grid.Input.SelectedElements;
        //
        //     if (powerUpSystem.TryGetPowerUp(selectedElements, out var powerUp))
        //     {
        //         // Power-up available
        //         yield return powerUpSystem.ExecutePowerUp(powerUp);
        //         MovesAvailable--;
        //         yield return ProcessGravity(); // Handle any cascades
        //     }
        //     else
        //     {
        //         // Normal match
        //         yield return grid.DespawnSelection();
        //         MovesAvailable--;
        //         yield return ProcessGravity();
        //     }
        //
        //     // Always respawn after any match
        //     yield return grid.RespawnElements();
        //     yield return ProcessGravity();
        // }
        
        
        private IEnumerator HandlePlayerMove()
        {
            yield return grid.WaitForSelection();

            var selectedElements = grid.Input.SelectedElements;
            bool powerUpWasUsed = false;

            // Power-up handling
            if (powerUpsEnabled && selectedElements.Count >= 4 && 
                powerUpSystem.TryGetPowerUp(selectedElements, out var powerUp))
            {
                yield return powerUp.Execute(); // Direct call without wrapper
                powerUpWasUsed = true;
        
                // Power-ups handle their own post-processing
                yield return ProcessGravity();
            }
            // Normal match handling
            else
            {
                yield return grid.DespawnSelection();
                MovesAvailable--;
                yield return ProcessGravity();
                yield return grid.RespawnElements();
                yield return ProcessGravity();
            }
        }

        // Example method to toggle power-ups from code
        public void SetPowerupsEnabled(bool enabled)
        {
            powerUpsEnabled = enabled;
            Debug.Log($"Power-ups {(enabled ? "enabled" : "disabled")}");
        }
        private IEnumerator HandleChainReactions()
        {
            do
            {
                yield return ProcessGravity();
                yield return grid.RespawnElements();
                yield return ProcessGravity();
            } 
            while (HasPotentialMatches());
        }

        private IEnumerator ProcessGravity()
        {
            bool needsMoreProcessing;
            do
            {
                yield return grid.WaitForMovement();
                needsMoreProcessing = CheckForFallingElements();
            } 
            while (needsMoreProcessing);
        }

        private bool CheckForFallingElements()
        {
            // Optimized version using grid array if available
            for (int y = 1; y < grid.RowCount; y++) // Start from 1 since bottom row can't fall
            {
                for (int x = 0; x < grid.ColumnCount; x++)
                {
                    var element = grid.GetElement(x, y);
                    if (element.IsSpawned && !element.IsMoving && 
                        !grid.GetElement(x, y-1).IsSpawned)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasPotentialMatches()
        {
            // Implement match detection logic here
            return false;
        }

        private void OnElementsDespawned(int count)
        {
            int newScore = Score + count * (count - 1);
            GameEvents.OnScoreChanged.Invoke(Score, newScore);
            Score = newScore;
        }

        private void OnBackButtonClick() { /* Implementation */ }
        private IEnumerator EndGame() { yield return new WaitForSeconds(0.5f); }
        private void Update() { if (Input.GetKey(KeyCode.Escape)) OnBackButtonClick(); }
    }
}