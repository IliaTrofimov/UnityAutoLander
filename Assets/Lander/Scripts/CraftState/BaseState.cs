using System;
using UnityEngine;

using Shared;

namespace CraftState
{
    public abstract class BaseState
	{
        public static float MaxMovementAfterTouch = 5;
        public static float MaxRotationAfterTouch = 20;
        public static float StandStillSeconds = 2;
        public static float MaxTocuhVelocity = 40;

        protected bool isFinalState;
        protected bool isStateChanged;
        protected MovementInfo movement;
        protected DateTime createdAt = DateTime.Now;

        public BaseState(MovementInfo movement, bool isStateChanged = true)
        {
            this.movement = movement;
            this.isStateChanged = isStateChanged;
        }

        public bool IsFinalState => isFinalState;
        public bool IsStateChanged => isStateChanged;

        public override string ToString()
        {
            return $"{GetType().Name,-15}\t{movement} {(isFinalState ? " !" : "")}";
        }

        public virtual BaseState NextState(MovementInfo newMovement)
        {
            isStateChanged = false;
            movement = newMovement;
            return this;
        }
    }
}