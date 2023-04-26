using UnityEngine;

namespace Assets.Scripts.GameInput
{
    [CreateAssetMenu(fileName = "PlayerControlScheme", menuName = "ScriptableObjects/PlayerControlScheme", order = 1)]
    public class PlayerControlScheme : ScriptableObject
    {
        public PlayerInputType inputType;
        
        [HideInInspector]
        public KeyCode moveUpKey = KeyCode.W;
        [HideInInspector]
        public KeyCode moveDownKey = KeyCode.S;
        [HideInInspector]
        public KeyCode moveLeftKey = KeyCode.A;
        [HideInInspector]
        public KeyCode moveRightKey = KeyCode.D;
    }
}