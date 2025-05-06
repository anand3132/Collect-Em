// GridMovement.cs
using System.Collections;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game
{
    public class GridMovement
    {
        private readonly Grid grid;
        private readonly IGridMovementSettings settings;

        public GridMovement(Grid grid, IGridMovementSettings settings = null)
        {
            this.grid = grid;
            this.settings = settings ?? new DefaultMovementSettings();
            if(settings==null)
                Debug.Log("Default movement settings");
        }

        public IEnumerator WaitForMovement()
        {
            MoveElements();

            while (!IsMovementDone())
            {
                yield return new WaitForSeconds(settings.MovementCheckInterval);
            }
            
        }

        private bool IsMovementDone()
        {
            foreach (GridElement element in grid.Elements)
            {
                if (element.IsSpawned && element.IsMoving)
                {
                    return false;
                }
            }
            return true;
        }

        private void MoveElements()
        {
            for (int y = 0; y < grid.RowCount; y++)
            {
                for (int x = 0; x < grid.ColumnCount; x++)
                {
                    ProcessCell(x, y);
                }
            }
        }

        private void ProcessCell(int column, int row)
        {
            GridElement element = grid.GetElement(column, row);

            if (!element.IsSpawned)
            {
                for (int i = row + 1; i < grid.RowCount; i++)
                {
                    GridElement next = grid.GetElement(column, i);
                    if (next != null && next.IsSpawned && !next.IsMoving)
                    {
                        MoveElement(new Vector2Int(column, i), new Vector2Int(column, row));
                        return;
                    }
                }
            }
        }

        private void MoveElement(Vector2Int oldPos, Vector2Int newPos)
        {
            GridElement element1 = grid.GetElement(oldPos.x, oldPos.y);
            GridElement element2 = grid.GetElement(newPos.x, newPos.y);

            grid.Elements[grid.GridPositionToIndex(oldPos)] = element2;
            grid.Elements[grid.GridPositionToIndex(newPos)] = element1;

            element2.transform.position = grid.GridToWorldPosition(oldPos);
            // Apply gravity multiplier to movement duration
            float adjustedDuration = settings.ElementMoveDuration / settings.GravityMultiplier;
            element1.Move(grid.GridToWorldPosition(newPos), adjustedDuration);        }

        // Default implementation when no designer is provided
        private class DefaultMovementSettings : IGridMovementSettings
        {
            public float MovementCheckInterval => 0.05f;
            public float ElementMoveDuration => 0.4f;
            public float PositionUpdateThreshold => 0.01f;
            public float GravityMultiplier => 1f; // Default normal speed
        }

    }
}