using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game.PowerUps
{
    [System.Serializable]
    public class PowerUpRule
    {
        public int MinimumElements;
        public Type PowerUpType;
    }

    public class PowerUpSystem
    {
        private Grid _grid;
        private List<PowerUpRule> _powerUpRules;

        public void Initialize(Grid grid, List<PowerUpRule> rules = null)
        {
            _grid = grid;
            _powerUpRules = rules ?? GetDefaultRules();
            _powerUpRules.Sort((a,b) => b.MinimumElements.CompareTo(a.MinimumElements));
        }

        private List<PowerUpRule> GetDefaultRules()
        {
            return new List<PowerUpRule>
            {
                // Your existing DoubleRocket at 4+ matches
                new PowerUpRule { 
                    MinimumElements = 4, 
                    PowerUpType = typeof(DoubleRocketPowerUp) 
                },
                // Add new rules below
                new PowerUpRule {
                    MinimumElements = 6,
                    PowerUpType = typeof(BombPowerUp)
                }
            };
        }

        
        public bool TryGetPowerUp(List<GridElement> selectedElements, out IPowerUp powerUp)
        {
            powerUp = null;
            if (selectedElements == null) return false;

            // Find the most powerful applicable power-up
            foreach (var rule in _powerUpRules)
            {
                if (selectedElements.Count >= rule.MinimumElements)
                {
                    var origin = _grid.GetGridPosition(selectedElements[0]);
                    powerUp = CreatePowerUpInstance(rule.PowerUpType, origin);
                    return true;
                }
            }
            return false;
        }

        private IPowerUp CreatePowerUpInstance(Type powerUpType, Vector2Int origin)
        {
            // Special handling for your DoubleRocketPowerUp
            if (powerUpType == typeof(DoubleRocketPowerUp))
            {
                return new DoubleRocketPowerUp(_grid, origin);
            }
            // Default creation for other power-ups
            return (IPowerUp)Activator.CreateInstance(powerUpType, _grid, origin);
        }
    }
}