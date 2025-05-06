using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedGaintGames.CollectEM.Game.PowerUps
{
    public class BombPowerUp : PowerUpBase
    {
        private GameObject _effectPrefab;
        
        public BombPowerUp(Grid grid, Vector2Int origin) : base(grid, origin) 
        {
            // Load effect prefab from Resources
            _effectPrefab = Resources.Load<GameObject>("PowerUps/BombExplosionEffect");
        }
    
        public override IEnumerator Execute()
        {
            // // Play explosion sound if available
            // if (GameSFX.Instance && GameSFX.Instance.ExplosionClip)
            // {
            //     GameSFX.Instance.Play(GameSFX.Instance.ExplosionClip);
            // }

            // Show visual effect if available
            if (_effectPrefab != null)
            {
                var effect = Object.Instantiate(
                    _effectPrefab,
                    _grid.GridToWorldPosition(_origin),
                    Quaternion.identity
                );
                Object.Destroy(effect, 1.5f);
            }

            // Clear 3x3 area around origin with bounds checking
            List<GridElement> elementsToClear = new List<GridElement>(9); // Pre-allocate for 3x3 grid
            
            for (int x = Math.Max(0, _origin.x-1); x <= Math.Min(_grid.ColumnCount-1, _origin.x+1); x++)
            {
                for (int y = Math.Max(0, _origin.y-1); y <= Math.Min(_grid.RowCount-1, _origin.y+1); y++)
                {
                    var element = _grid.GetElement(x, y);
                    if (element != null && element.IsSpawned) 
                    {
                        elementsToClear.Add(element);
                        // Optional: Play pre-destruction effect
                        // element.PlayHighlightEffect(Color.red, 0.3f);
                    }
                }
            }

            // Despawn all affected elements
            yield return _grid.Despawn(elementsToClear);
            
            // Wait for animations to complete
            yield return _grid.WaitForMovement();

            // Optional: Camera shake for impact
            // if (Camera.main.TryGetComponent<CameraShake>(out var shaker))
            // {
            //     yield return shaker.Shake(0.2f, 0.4f);
            // }
        }
    }
}