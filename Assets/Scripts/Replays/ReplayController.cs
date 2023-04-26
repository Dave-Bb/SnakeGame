using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Replays
{
    /// <summary>
    /// Bare bones replay controller that could be used.
    ///
    /// The idea could be to record the updated position/direction changes for a snake.
    ///
    /// Then when the playback is happening, control a snake with these saved positions/directions
    /// on each tick.
    ///
    /// The apple position and spawning is not really fleshed out. 
    /// </summary>
    public class ReplayController : MonoBehaviour
    {
        private Queue<Vector2Int> savedPosition = new Queue<Vector2Int>();

        public Vector2Int GetNextRecordedPosition()
        {
            if (savedPosition.TryDequeue(out Vector2Int pos))
            {
                return pos;
            }
            
            return Vector2Int.zero;
        }
        
        public void RecordPositionForTick(Vector2Int position)
        {
            savedPosition.Enqueue(position);
        }
    }
}