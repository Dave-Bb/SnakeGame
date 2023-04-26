using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.GameInput;
using UnityEngine;

namespace Assets.Scripts.Snakes
{
    public abstract class Snake : MonoBehaviour, ISnakeController
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] protected PlayerControlScheme controls;

        protected Vector2Int nextDirection;
        
        private SnakeBoard board;
        private Color snakeColor;
        private LinkedList<GameObject> snakeBody;
        private List<Transform> snakePositions = new List<Transform>(); //This could be used to play back the snake?
        public int SnakeId { get; private set; }

        public void Awake()
        {
            snakeBody = new LinkedList<GameObject>();
            nextDirection = Vector2Int.down;

            if (controls == null)
            {
                Debug.LogError($"{nameof(SnakePlayer)} Does not have a {nameof(PlayerControlScheme)}. Please add one. Disabling snake");
                enabled = false;
            }
        }
        
        public void InitSnake(Vector2Int startPosition, PlayerSettings playerSettings, SnakeBoard newBoard)
        {
            board = newBoard;
            SnakeId = playerSettings.id;
            snakeColor = playerSettings.color;
            controls = playerSettings.controlScheme;
            
            for (int i = 0; i < 4; i++)
            {
                AddSnakeCell(startPosition - new Vector2Int(0, i));
            }
        }

        public virtual void HandleMovement() { }

        public virtual bool MoveUp()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool MoveDown()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool MoveLeft()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool MoveRight()
        {
            throw new System.NotImplementedException();
        }
        
        private void AddSnakeCell(Vector2Int position)
        {
            GameObject newCell = Instantiate(cellPrefab, new Vector3((int)position.x, (int)position.y, 0), Quaternion.identity, transform);
            newCell.GetComponent<SpriteRenderer>().color = snakeColor;
            snakeBody.AddFirst(newCell);
            board.UpdateBoardCell(position, 1);
        }

        public Vector2Int GetNextSnakePosition()
        {
            return GetHeadPosition() + nextDirection;
        }

        public Vector2Int GetNextSnakePosition(Vector2Int playbackDirection)
        {
            return GetHeadPosition() + playbackDirection;
        }

        private Vector2Int GetHeadPosition()
        {
            return new Vector2Int((int)snakeBody.First.Value.transform.position.x, (int)snakeBody.First.Value.transform.position.y);
        }
        
        public void AddSnakeCellAtNewPosition(Vector2Int newPosition)
        {
            AddSnakeCell(newPosition);
        }
        
        public void MoveSnake(Vector2Int newPosition)
        {
            GameObject tail = snakeBody.Last.Value;
            Vector2Int tailPosition = new Vector2Int((int)tail.transform.position.x, (int)tail.transform.position.y);
            board.UpdateBoardCell(tailPosition, 0);

            tail.transform.position = new Vector3(newPosition.x, newPosition.y, 0);
            snakeBody.RemoveLast();
            snakeBody.AddFirst(tail);
    
            board.UpdateBoardCell(newPosition, 1);
        }

    }
}