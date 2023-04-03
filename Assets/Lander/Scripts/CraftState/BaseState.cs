using System;
using UnityEngine;

using Shared;

namespace CraftState
{
    public abstract class BaseState
	{
        protected bool isStateChanged;
        protected MovementInfo movement;
        protected DateTime createdAt = DateTime.Now;

        public BaseState(MovementInfo movement, bool isStateChanged = false)
        {
            this.movement = movement;
            this.isStateChanged = isStateChanged;
        }

        public bool IsStateChanged => isStateChanged;

        public override string ToString()
        {
            return $"{GetType().Name.ToString()}   {movement}";
        }

        public virtual BaseState NextState(MovementInfo newMovement)
        {
            isStateChanged = false;
            movement = newMovement;
            return this;
        }
    }
}