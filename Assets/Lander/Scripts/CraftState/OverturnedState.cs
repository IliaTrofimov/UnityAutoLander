using UnityEngine;
using Lander.Shared;

namespace Lander.CraftState
{
    public class OverturnedState : FatalState
    {
        public OverturnedState(MovementInfo movement, StateSettings settings, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (newMovement.IsCollided)
                return new CrashedState(newMovement, settings);
            else if (Vector3.Dot(newMovement.Normal, Vector3.up) >= -0.35f)
                return new FlyingState(newMovement, settings);
            else
                return base.NextState(newMovement);
        }
    }
}