using UnityEngine;

namespace Assets.Scripts.Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("Player Settings")]
        public PlayerSettings playerOne;
        public PlayerSettings playerTwo;

        [Header("Board Settings")]
        public Color backgroundColor;
        public Color appleColor;
        public int width;
        public int height;
        public float gameSpeed;

        [Header("Prefabs")] 
        public GameObject cellPrefab;
        public GameObject snakeCellPrefab;
        
        private void OnValidate()
        {
            if (width <= 0)
            {
                LogInvalidValue(nameof(width));
            }
            
            if (height <= 0)
            {
                LogInvalidValue(nameof(height));
            }
            
            if (gameSpeed <= 0)
            {
                LogInvalidValue(nameof(gameSpeed));
            }

            if (backgroundColor.a < 0.5f)
            {
                LogColorAlphaWarning(nameof(backgroundColor));
            }
            
            if (appleColor.a < 0.5f)
            {
                LogColorAlphaWarning(nameof(appleColor));
            }

            if (cellPrefab == null)
            {
                LogNullValue(nameof(cellPrefab));
            }
            
            if (snakeCellPrefab == null)
            {
                LogNullValue(nameof(snakeCellPrefab));
            }
            //Add more validations...
        }

        private void LogInvalidValue(string fieldName)
        {
            Debug.LogError($"{nameof(GameSettings)} Invalid Field {fieldName}. Can not be less than or equal to zero");
        }

        private void LogNullValue(string fieldName)
        {
            Debug.LogError($"{nameof(GameSettings)} WARNING: {nameof(fieldName)} is Null");
        }

        private void LogColorAlphaWarning(string fieldName)
        {
            Debug.LogError($"{nameof(GameSettings)} WARNING: {nameof(fieldName)} has a very low Alpha value");
        }
    }
}