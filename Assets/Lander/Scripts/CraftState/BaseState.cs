using System;
using UnityEngine;

using Lander.Shared;

namespace Lander.CraftState
{
    public abstract class BaseState
	{
        public bool IsStateChanged => isStateChanged;
        public string Name => GetType().Name.Replace("State", "");
        public MovementInfo Movement => movement;

        protected StateSettings settings;
        protected bool isStateChanged;
        protected MovementInfo movement;
        protected DateTime createdAt = DateTime.Now;


        public BaseState(MovementInfo movement, StateSettings settings = default, bool isStateChanged = true)
        {
            this.movement = movement;
            this.isStateChanged = isStateChanged;
            this.settings = settings;
        }


        public virtual BaseState NextState(MovementInfo newMovement)
        {
            isStateChanged = false;
            movement = newMovement;
            return this;
        }

        public override string ToString() => GetType().Name.Replace("State", "");
    }
}