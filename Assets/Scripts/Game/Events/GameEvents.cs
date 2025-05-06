using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

namespace RedGaintGames.CollectEM.Game
{
    /// <summary>
    /// UnityEvent with integer argument
    /// </summary>
    public class UnityIntEvent : UnityEvent<int> { }

    /// <summary>
    /// UnityEvent with two integer arguments
    /// </summary>
    public class ScoreChangedEvent : UnityEvent<int, int> { }

    /// <summary>
    /// UnityEvent with score data for animations
    /// </summary>
    public class ScoreAnimationEvent : UnityEvent<int, List<Vector3>> { }

    /// <summary>
    /// Static class holding all relevant game events
    /// </summary>
    public static class GameEvents
    {
        // Existing Events
        public static UnityIntEvent OnElementsDeSpawned { get; } = new UnityIntEvent();
        public static UnityIntEvent OnSelectionChanged { get; } = new UnityIntEvent();
        public static ScoreChangedEvent OnScoreChanged { get; } = new ScoreChangedEvent();

        // New Events for Score Animation
        public static ScoreAnimationEvent OnScoreAnimationRequested { get; } = new ScoreAnimationEvent();
        public static UnityEvent OnScoreReachedCounter { get; } = new UnityEvent();
    }
}