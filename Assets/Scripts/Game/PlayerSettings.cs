using System;
using Assets.Scripts.GameInput;
using UnityEngine;

namespace Assets.Scripts.Game
{
    [Serializable]
    public class PlayerSettings
    {
        public Color color;
        public PlayerControlScheme controlScheme;
        public int id;
    }
}