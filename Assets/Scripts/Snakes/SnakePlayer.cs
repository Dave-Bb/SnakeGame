using System;
using Assets.Scripts.GameInput;
using UnityEngine;

namespace Assets.Scripts.Snakes
{
    public class SnakePlayer : Snake
    {
        public override void HandleMovement()
        {
            if (MoveUp() && nextDirection != Vector2Int.down)
            {
                nextDirection = Vector2Int.up;
            }
            else if (MoveDown() && nextDirection != Vector2Int.up)
            {
                nextDirection = Vector2Int.down;
            }
            else if (MoveLeft() && nextDirection != Vector2Int.right)
            {
                nextDirection = Vector2Int.left;
            }
            else if (MoveRight() && nextDirection != Vector2Int.left)
            {
                nextDirection = Vector2Int.right;
            }
        }

        /*
         * Handle the input based off the current input scheme.
         *
         * These methods if more input types are added, these should just return a bool if this direction is what is inputted. 
         */
        public override bool MoveUp()
        {
            switch (controls.inputType)
            {
                case PlayerInputType.Keyboard:
                    return Input.GetKeyDown(controls.moveUpKey);
                
                case PlayerInputType.Gamepad:
                case PlayerInputType.Touch:
                    Debug.LogError("Input type not implemented.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }
        
        public override bool MoveDown()
        {
            switch (controls.inputType)
            {
                case PlayerInputType.Keyboard:
                    return Input.GetKeyDown(controls.moveDownKey);
                case PlayerInputType.Gamepad:
                case PlayerInputType.Touch:
                    Debug.LogError("Input type not implemented.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }
        
        public override bool MoveLeft()
        {
            switch (controls.inputType)
            {
                case PlayerInputType.Keyboard:
                    return Input.GetKeyDown(controls.moveLeftKey);
                case PlayerInputType.Gamepad:
                case PlayerInputType.Touch:
                    Debug.LogError("Input type not implemented.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }
        
        public override bool MoveRight()
        {
            switch (controls.inputType)
            {
                case PlayerInputType.Keyboard:
                    return Input.GetKeyDown(controls.moveRightKey);
                case PlayerInputType.Gamepad:
                case PlayerInputType.Touch:
                    Debug.LogError("Input type not implemented.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }
    }
}