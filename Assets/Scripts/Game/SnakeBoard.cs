using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.Replays;
using Assets.Scripts.Snakes;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeBoard : MonoBehaviour
{
   [SerializeField] private PlayerCount playerCount;
   [SerializeField] private GameSettings gameSettings;
   [SerializeField] private SpriteRenderer boundaryWallSpriteRenderer;
   [SerializeField] private KeyCode RestartKey = KeyCode.Return;
   [SerializeField] private KeyCode PlayLastGameKey = KeyCode.K;
   
   private int boardWidth = 20;
   private int boardHeight = 20;
   private int cellWidth = 1;
   private int[,] board;
   
   private GameObject apple;
   private SnakePlayer snakePlayer;
   private GameState gameState;
   private float gameTickSpeed;
   private List<Snake> snakes = new List<Snake>();
   private float timeSinceLastTick;
   private int occupiedCells;
   private int maxOccupiedCells;
   private Snake removeSnakeOnNextTick;

   private ReplayController replayController;
   private float HalfCellWidth => cellWidth * 0.5f;

   private const float DefaultSpeed = 10f;
   private const float HalfMultiplier = 0.5f;
   private const float CameraZ = -10f;
   private const float PlayerOneSpawn = 0.5f;
   private const float PlayerTwoSpawn = 0.2f;

   private void Awake()
   {
      if (gameSettings == null)
      {
         Debug.LogError($"{nameof(SnakeBoard)} No Game Settings found! Game will not run. Set up a {nameof(GameSettings)}");
         enabled = false;
         return;
      }

      SetUpGame();

      replayController = gameObject.AddComponent<ReplayController>();
   }
   
   private void Start()
   {
      SizeBoundaryWall();
      SetUpCamera();
   }

   private void Update()
   {
      switch (gameState)
      {
         case GameState.Playing:
            HandleInput(snakes);
            HandleGameTick(snakes);
            break;
         case GameState.GameOver:
            if (Input.GetKeyDown(RestartKey))
            {
               RestartGame();
            }
            if (Input.GetKeyDown(PlayLastGameKey))
            {
               gameState = GameState.Playback;
               RestartGame();
            }
            break;
         case GameState.Playback:
            if (Input.GetKeyDown(RestartKey))
            {
               RestartGame();
            }
            
            //Handle playback
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
   
   /// <summary>
   /// Update the target board cell value. 
   /// </summary>
   /// <param name="position">The location of the target cell</param>
   /// <param name="value">1 if the cell is now active, 0 if it is free</param>
   public void UpdateBoardCell(Vector2Int position, int value)
   {
      board[position.x, position.y] = value;
      occupiedCells += value == 1 ? 1 : -1; //once this is >= max cells, then the board is full of snakes. 
   }
   
   private void SetUpGame()
   {
      gameState = GameState.GameOver; //Game just starts as GameOver. 
      
      gameTickSpeed = gameSettings.gameSpeed;
            
      if (gameTickSpeed <= 0)
      {
         Debug.LogError($"{nameof(SnakeBoard)} Warning! Your game tick speed is <= ZERO. Setting default value.");
         gameTickSpeed = DefaultSpeed;
      }

      boardWidth = gameSettings.width;
      boardHeight = gameSettings.height;

      maxOccupiedCells = boardHeight * boardWidth; //when the game is full
      occupiedCells = 0;
      
      board = new int[boardWidth, boardHeight];
   }
   
   private void SetUpCamera()
   {
      //This is just to make sure that the game can still fit with in the camera. 
      Camera mainCamera = Camera.main;
      mainCamera.transform.position = new Vector3(boardWidth * HalfMultiplier, (boardHeight * HalfMultiplier) - HalfCellWidth, CameraZ);
      mainCamera.orthographicSize = (boardWidth * HalfMultiplier) + cellWidth;
      
      mainCamera.backgroundColor = gameSettings.backgroundColor;
   }

   private void SizeBoundaryWall()
   {
      if (boundaryWallSpriteRenderer == null)
      {
         Debug.LogError($"Boundary wall sprite is null.");
         return;
      }

      boundaryWallSpriteRenderer.size = new Vector2(boardWidth + cellWidth, boardHeight + cellWidth);
      boundaryWallSpriteRenderer.transform.localPosition = new Vector3((boardWidth * HalfMultiplier) - HalfCellWidth , (boardHeight * HalfMultiplier) - HalfCellWidth, 0f);
   }

   private void RestartGame()
   {
      board = new int[boardWidth, boardHeight];

      SetUpSnakes();
      SetUpApple();

      gameState = GameState.Playing;

      occupiedCells = 0;
   }

   private void SetUpApple()
   {
      if (apple != null)
      {
         Destroy(apple);
      }
      
      SpawnApple();
   }

   private void SetUpSnakes()
   {
      foreach (var snake in snakes)
      {
         Destroy(snake.gameObject);
      }
      
      snakes.Clear();
      
      MakeNewSnake(snakes, new Vector2Int((int)(boardHeight * PlayerOneSpawn), (int)(boardWidth * PlayerOneSpawn)), 
         gameSettings.playerOne, gameSettings.snakeCellPrefab);

      if (playerCount == PlayerCount.Two)
      {
         MakeNewSnake(snakes, new Vector2Int((int)(boardHeight * PlayerTwoSpawn), (int) (boardWidth * PlayerTwoSpawn)),
            gameSettings.playerTwo, gameSettings.snakeCellPrefab);
      }
      
      //How many snakes is too many snakes. I think its possible to just keep adding them, if they have their own settings. 
   }

   private void MakeNewSnake(List<Snake> snakes, Vector2Int startingPos, PlayerSettings playerSettings, GameObject celPrefab)
   {
      var snakeOne = Instantiate(celPrefab).GetComponent<SnakePlayer>();
      snakeOne.InitSnake(startingPos, playerSettings, this);
      snakes.Add(snakeOne);
   }

   private void HandleGameTick(List<Snake> snakes)
   {
      if (timeSinceLastTick > 1 / gameTickSpeed)
      {
         switch (gameState)
         {
            case GameState.Playing:
               foreach (var snake in snakes)
               {
                  if (snake == null)
                  {
                     continue;
                  }
                  
                  RunGameLoop(snake);
               }

               //Remove dead snakes
               if (removeSnakeOnNextTick != null)
               {
                  snakes.Remove(removeSnakeOnNextTick);
                  Destroy(removeSnakeOnNextTick.gameObject);
               }
               break;
            case GameState.GameOver:
               break;
            case GameState.Playback:
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
         

         timeSinceLastTick = 0f;
      }

      timeSinceLastTick += Time.deltaTime;
   }

   private void HandleInput(List<Snake> snakes)
   {
      foreach (var snake in snakes)
      {
         snake.HandleMovement();
      }
   }

   private void RunGameLoop(Snake snake)
   {
      Vector2Int newPosition = snake.GetNextSnakePosition(); 
      
      //Possibly record the position for this snake to use in Playback/Replays

      // Check for collisions
      if (newPosition.x < 0 || newPosition.x >= boardWidth || newPosition.y < 0 || newPosition.y >= boardHeight)
      {
         // Boundary collision
         Debug.LogError($"Game over at {newPosition} for snake {snake.SnakeId}");
         RemoveDeadSnake(snake.SnakeId);
         return;
      }

      if (board[newPosition.x, newPosition.y] == 1)
      {
         // Self collision
         Debug.LogError($"Game over at {newPosition} for snake {snake.SnakeId}");
         RemoveDeadSnake(snake.SnakeId);
         return;
      }

      if (occupiedCells >= maxOccupiedCells)
      {
         //Board is full.
         GameOver();
         return;
      }

      //If we eat an apple, the apple becomes the new head for the snake. 
      if (newPosition == GetApplePosition())
      {
         // Apple collision
         snake.AddSnakeCellAtNewPosition(newPosition);
        
         Destroy(apple);
         SpawnApple();
      }
      else //Move the snake along to the new position 
      {
         snake.MoveSnake(newPosition);
      }
   }
   
   private Vector2Int GetApplePosition()
   {
      return new Vector2Int((int)apple.transform.position.x, (int)apple.transform.position.y);
   }
   
   private void SpawnApple()
   {
      Vector2Int applePosition;

      do
      {
         applePosition = new Vector2Int(Random.Range(0, boardWidth), Random.Range(0, boardHeight));
      } while (board[applePosition.x, applePosition.y] == 1);

      apple = Instantiate(gameSettings.cellPrefab, new Vector3(applePosition.x, applePosition.y, 0), Quaternion.identity);
      apple.name = "Apple";
      apple.GetComponent<SpriteRenderer>().color = gameSettings.appleColor;
      
      //Record the position of this apple for use in replays here possibly. 
   }
   
   private void GameOver()
   {
      gameState = GameState.GameOver;
      // Implement game over logic, e.g., restart game, show game over screen, etc.
      Debug.Log("Game Over!");
   }

   private void RemoveDeadSnake(int snakeId)
   {
      if (snakes.Count > 1)
      {
         removeSnakeOnNextTick = null;
         foreach (var snake in snakes)
         {
            if (snake.SnakeId == snakeId)
            {
               removeSnakeOnNextTick = snake; //This is the losing snake in this context
            }
         }
      }
      else
      {
         GameOver();
      }
   }
}
