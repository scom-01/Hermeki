using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionMovement : ActionData
    {
        [field: SerializeField] public MovementData[] movements { get; private set; }

        [Serializable]
        public struct MovementData
        {
            public bool CanFlip;
            public bool CanMoveCtrl ;
            public Vector2 Direction ;
            public float Velocity ;
        }    
    }
}