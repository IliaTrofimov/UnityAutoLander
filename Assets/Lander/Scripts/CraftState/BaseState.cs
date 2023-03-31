using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CraftState
{
    public abstract class BaseState
	{
        protected MovementInfo movement;
        protected DateTime createdAt = DateTime.Now;

        public BaseState(MovementInfo movement)
        {
            this.movement = movement;
        }

        public override string ToString()
        {
            return this.GetType().Name.Replace("State", "");
        }

        public virtual BaseState NextState(MovementInfo newMovement)
        {
            movement = newMovement;
            return this;
        }
    }
}