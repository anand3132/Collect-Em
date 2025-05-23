﻿namespace RedGaintGames.CollectEM.Game
{
    using UnityEngine;

    /// <summary>
    /// Extension class of the game grid, containing some helper methods
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Gets the element of the grid in the given column and row 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static GridElement GetElement(this Grid grid, int column, int row)
        {
            return grid.Elements[row * grid.ColumnCount + column];
        }

        /// <summary>
        /// Gets the world position of the given position in the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Vector2 GridToWorldPosition(this Grid grid, int column, int row)
        {
            return grid.StartCellPosition + grid.CellSize * new Vector2(column, row);
        }

        /// <summary>
        /// Gets the world position of the given position in the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public static Vector2 GridToWorldPosition(this Grid grid, Vector2Int gridPos)
        {
            return grid.GridToWorldPosition(gridPos.x, gridPos.y);
        }

        /// <summary>
        /// Gets the index of the element at the given grid position
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        public static int GridPositionToIndex(this Grid grid, Vector2Int gridPos)
        {
            return grid.ColumnCount * gridPos.y + gridPos.x;
        }
        
        public static Vector2Int GridPositionForElement(this Grid grid, GridElement element)
        {
            for (int y = 0; y < grid.RowCount; y++)
            {
                for (int x = 0; x < grid.ColumnCount; x++)
                {
                    if (grid.GetElement(x, y) == element)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return Vector2Int.zero;
        }
    }
}