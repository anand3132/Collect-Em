﻿namespace RedGaintGames.CollectEM.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Processed the user input on the game grid
    /// </summary>
    public class GridInput
    {
        private Grid grid;
        private SelectionLine selectionLine;
        private int backtrackStepIndex = 2;
        public List<GridElement> SelectedElements { get; private set; }

        public GridInput(Grid grid, SelectionLine selectionLine)
        {
            this.grid = grid;
            this.selectionLine = selectionLine;
            this.SelectedElements = new List<GridElement>(); ;
        }

        /// <summary>
        /// Clears the current selection
        /// </summary>
        private void Clear()
        {
            this.selectionLine.Clear();
            this.SelectedElements.Clear();

            GameEvents.OnSelectionChanged.Invoke(this.SelectedElements.Count);
        }

        /// <summary>
        /// Processes the user input and waits until a selection has been made
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForSelection()
        {
            Clear();

            while (true)
            {
                yield return null;

                //Does the user touch the screen?
                if (Input.GetMouseButton(0))
                {
                    //Does the user touch an element?
                    if (InputRaycast(out GridElement element))
                    {
                        ProcessGridElement(element);
                    }
                }
                //Selection finished?
                else if (this.SelectedElements.Count > 0)
                {
                    if (IsValidInput())
                    {
                        //Clear selection line when finished
                        this.selectionLine.Clear();

                        //Stop coroutine
                        yield break;
                    }
                    else
                    {
                        Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if an element is touched by the mouse, false otherwise.
        /// Outs the touched element, null otherwise
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool InputRaycast(out GridElement element)
        {
            element = null;

            //Get mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Shoot a raycast on z-level
            RaycastHit2D hitInfo = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hitInfo)
            {
                //Try to get the gamegrid element
                element = hitInfo.transform.GetComponent<GridElement>();

                //Return true if the hit object is GameGridElement
                if (element != null)
                {
                    return true;
                }
            }

            //Return false if nothing is hit or if the object is not GameGridElement
            return false;
        }

        /// <summary>
        /// Processes the input on the given element
        /// </summary>
        /// <param name="element"></param>
        private void ProcessGridElement(GridElement element)
        {
            //No element selected yet? Select element!
            if (this.SelectedElements.Count == 0)
            {
                //Cache selected color
                this.selectionLine.Color = element.Color;

                //Add element to selection
                AddElementToSelection(element);
            }
            else
            {
                //Is the element already selected?
                if (this.SelectedElements.Contains(element))
                {
                    //Did the player moved back? Deselect element!
                    if (IsSecondLast(element))
                    {
                        DeselectLast();
                    }
                }
                //Not selected, correct color and in distance? Select element!
                else if (IsSelectable(element))
                {
                    AddElementToSelection(element);
                }
            }
        }

        /// <summary>
        /// Returns true if the given element is the second last in the selection,
        /// false otherwise. This is used to determine the deselection of selected elements.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsSecondLast(GridElement element)
        {
            return this.SelectedElements.Count >= backtrackStepIndex && element.Equals(this.SelectedElements[this.SelectedElements.Count - backtrackStepIndex]) == true;
        }

        /// <summary>
        /// Returns true if the given element can be selected,
        /// false otherwise
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsSelectable(GridElement element)
        {
            return HasValidColor(element) && IsInDistance(element);
        }

        /// <summary>
        /// Returns true if the given element has the current selection color,
        /// false otherwise
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool HasValidColor(GridElement element)
        {
            return element.Color == this.selectionLine.Color;
        }

        public Vector3 GetSelectionCenterPosition()
        {
            if (SelectedElements == null || SelectedElements.Count == 0)
                return Vector3.zero;

            Vector3 center = Vector3.zero;
            foreach (var element in SelectedElements)
            {
                center += element.transform.position;
            }
            return center / SelectedElements.Count;
        }

        public Vector2Int GetSelectionGridCenter()
        {
            if (SelectedElements == null || SelectedElements.Count == 0)
                return Vector2Int.zero;

            Vector2Int center = Vector2Int.zero;
            foreach (var element in SelectedElements)
            {
                center += grid.GetGridPosition(element);
            }
            return center / SelectedElements.Count;
        }
        /// <summary>
        /// Returns true if the given element is adjacent to the last element in the selection,
        /// false otherwise
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsInDistance(GridElement element)
        {
            Vector2 lastElementPos = this.SelectedElements.Last().transform.position;

            return Vector2.Distance(element.transform.position, lastElementPos) < 1.5f * this.grid.CellSize;
        }

        /// <summary>
        /// Adds the given element to the selection
        /// </summary>
        /// <param name="selectedElements"></param>
        /// <param name="element"></param>
        private void AddElementToSelection(GridElement element)
        {
            if (this.SelectedElements.Count > 0)
            {
                float pitch = 1.0f + this.SelectedElements.Count * 0.1f;
                GameSFX.Instance.Play(GameSFX.Instance.SelectionClip, pitch);
            }

            this.SelectedElements.Add(element);

            this.selectionLine.Color = element.Color;

            this.selectionLine.SetPositions(this.SelectedElements);

            GameEvents.OnSelectionChanged.Invoke(this.SelectedElements.Count);
        }

        /// <summary>
        /// Removes the last element from the selection
        /// </summary>
        /// <param name="selectedElements"></param>
        private void DeselectLast()
        {
            this.SelectedElements.Remove(this.SelectedElements.Last());

            this.selectionLine.SetPositions(this.SelectedElements);

            GameEvents.OnSelectionChanged.Invoke(this.SelectedElements.Count);
        }

        /// <summary>
        /// Returns true if more than one element is selected, false otherwise
        /// </summary>
        /// <returns></returns>
        private bool IsValidInput()
        {
            return this.SelectedElements.Count >= grid.MatchMin ? true : false;
        }
        
        private Vector3 GetMatchCenterPosition(List<GridElement> matchedElements)
        {
            Vector3 center = Vector3.zero;
            foreach (var element in matchedElements)
            {
                center += element.transform.position;
            }
            return center / matchedElements.Count;
        }
    }
}