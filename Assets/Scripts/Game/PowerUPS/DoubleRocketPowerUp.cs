using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game.PowerUps
{
    public class DoubleRocketPowerUp : PowerUpBase
    {
        private GameObject _effectPrefab;
        
        public DoubleRocketPowerUp(Grid grid, Vector2Int origin) : base(grid, origin) 
        {
            // Load effect prefab from Resources
            _effectPrefab = Resources.Load<GameObject>("PowerUps/DoubleRocketEffect");
        }

        public override IEnumerator Execute()
        {
            // // Play power-up sound
            // if (GameSFX.Instance && GameSFX.Instance.PowerUpClip)
            // {
            //     GameSFX.Instance.Play(GameSFX.Instance.PowerUpClip);
            // }

            // Show visual effect if available
            if (_effectPrefab != null)
            {
                var effect = Object.Instantiate(
                    _effectPrefab,
                    _grid.GridToWorldPosition(_origin),
                    Quaternion.identity
                );
                Object.Destroy(effect, 1.5f); // Auto-destroy after animation
            }

            // Get all elements in row and column
            List<GridElement> elementsToClear = new List<GridElement>(_grid.ColumnCount + _grid.RowCount - 1);
            
            // Horizontal sweep (entire row)
            for (int x = 0; x < _grid.ColumnCount; x++)
            {
                TryAddElement(x, _origin.y, elementsToClear);
            }
            
            // Vertical sweep (skip origin since it was already added)
            for (int y = 0; y < _grid.RowCount; y++)
            {
                if (y != _origin.y) 
                {
                    TryAddElement(_origin.x, y, elementsToClear);
                }
            }

            // Despawn all affected elements and wait for completion
            yield return base.DespawnElements(elementsToClear);

            // // Optional: Add screen shake effect for impact
            // if (Camera.main.TryGetComponent<CameraShake>(out var shaker))
            // {
            //     shaker.Shake(0.15f, 0.3f);
            // }
        }

        private void TryAddElement(int x, int y, List<GridElement> list)
        {
            var element = _grid.GetElement(x, y);
            if (element != null && element.IsSpawned)
            {
                list.Add(element);
                
                // Optional: Play a sparkle effect on each element
                // element.PlayHighlightEffect(Color.white, 0.5f);
            }
        }
    }
}