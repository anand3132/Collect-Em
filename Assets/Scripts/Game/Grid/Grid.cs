﻿namespace RedGaintGames.CollectEM.Game
{
    using CollectEM.Core.Extensions;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The grid holding all elements/bubbles
    /// </summary>
    public class Grid : MonoBehaviour, IGridPositionProvider
    {
        [SerializeField] private SelectionLine selectionLine = null;
        [SerializeField] private GridElement gridElementPrefab = null;

        [SerializeField] private int rowCount = 8;
        [SerializeField] private int columnCount = 8;
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private int matchMinimum = 3;
        [SerializeField] private List<Color> colors = new List<Color>();

        public  GridInput gridInput;
        private GridMovement gridMovement;
        private GridSpawning gridSpawning;

        /// <summary>
        /// Gets the list of all elements in the grid
        /// </summary>
        public List<GridElement> Elements { get; } = new List<GridElement>();

        /// <summary>
        /// Gets the list of available colors
        /// </summary>
        public List<Color> Colors => this.colors;

        /// <summary>
        /// Returns the center (worldposition) of the most left cell on the bottom 
        /// </summary>
        public Vector2 StartCellPosition => this.GridCenter - this.CellSize * new Vector2(this.ColumnCount - 1, this.RowCount - 1) / 2.0f;

        /// <summary>
        /// Returns the size of a single cell
        /// </summary>
        public float CellSize => this.cellSize;

        /// <summary>
        /// Returns the number of rows (=vertical length)
        /// </summary>
        public int RowCount => this.rowCount;

        /// <summary>
        /// Returns the number of columns (=horizontal length)
        /// </summary>
        public int ColumnCount => this.columnCount;

        /// <summary>
        /// Returns the center poistion of the grid
        /// </summary>
        public Vector2 GridCenter => this.transform.position;

        /// <summary>
        /// Returns the Transform containing all grid elements 
        /// </summary>
        public Transform GridContainer => this.transform;
        /// <summary>
        /// The minimum number of connected cells required to form a valid match.
        /// </summary>
        public int MatchMin => this.matchMinimum;

        private void Start()
        {
            this.gridInput = new GridInput(this, this.selectionLine);
            this.gridMovement = new GridMovement(this,GameManager.Instance.GridDesignerUI);
            this.gridSpawning = new GridSpawning(this,GameManager.Instance.GridDesignerUI);
        }

        /// <summary>
        /// Creates all elements of the grid and sets them up
        /// </summary>
        public void GenerateGrid()
        {
            // Get designer reference if available
            // var designer = FindObjectOfType<GridDesignerUI>();
    
            for (int y = 0; y < this.ColumnCount; y++)
            {
                for (int x = 0; x < this.RowCount; x++)
                {
                    GridElement element = Instantiate(this.gridElementPrefab);

                    // Initialize with designer settings if available
                    element.Initialize(GameManager.Instance.GridDesignerUI);

                    // Set physical properties
                    element.transform.localScale = Vector2.one * this.cellSize;
                    element.transform.position = this.GridToWorldPosition(x, y);
                    element.transform.SetParent(this.GridContainer.transform, true);
            
                    // Set visual properties
                    element.Color = this.colors.GetRandom();

                    this.Elements.Add(element);
                }
            }
        }

        /// <summary>
        /// Returns a coroutine waiting until all grid elements are not moving
        /// </summary>
        /// <returns></returns>
        public Coroutine WaitForMovement()
        {
            return StartCoroutine(this.gridMovement.WaitForMovement());
        }

        /// <summary>
        /// Returns a coroutine waiting until the user has made a selection
        /// </summary>
        /// <returns></returns>
        public Coroutine WaitForSelection()
        {
            return StartCoroutine(this.gridInput.WaitForSelection());
        }

        /// <summary>
        /// Returns a coroutine respawning all despawned elements and waiting until they are
        /// finished
        /// </summary>
        /// <returns></returns>
        public Coroutine RespawnElements()
        {
            return StartCoroutine(this.gridSpawning.RespawnElements());
        }

        /// <summary>
        /// Returns a coroutine despawning the selected elements and waiting until they are finished
        /// </summary>
        /// <returns></returns>
        public Coroutine DespawnSelection()
        {
            return StartCoroutine(this.gridSpawning.Despawn(this.gridInput.SelectedElements));
        }
        /// <summary>
        /// Public access to grid input system
        /// </summary>
        public GridInput Input => gridInput;

        /// <summary>
        /// Despawns a list of elements (delegates to GridSpawning)
        /// </summary>
        public Coroutine Despawn(List<GridElement> elements)
        {
            return StartCoroutine(gridSpawning.Despawn(elements));
        }

        /// <summary>
        /// Gets the world position for a grid coordinate (overload)
        /// </summary>
        public Vector2 GridToWorldPosition(Vector2Int gridPos)
        {
            return StartCellPosition + new Vector2(gridPos.x * CellSize, gridPos.y * CellSize);
        }

        /// <summary>
        /// Gets the grid element at specified coordinates with bounds checking
        /// </summary>
        public GridElement GetElement(int x, int y)
        {
            if (x < 0 || x >= ColumnCount || y < 0 || y >= RowCount)
                return null;
            
            return Elements[y * ColumnCount + x];
        }

        public Vector2Int GetGridPosition(GridElement element)
        {
            int index = Elements.IndexOf(element);
            return new Vector2Int(
                index % ColumnCount, 
                index / ColumnCount
            );
        }
    }
}