using System;
using UnityEngine;
using Lander.Shared;

namespace Lander.CraftState
{
    public class FixationState : BaseState
    {
        private DateTime touchedAt;
        private Vector3 touchPosition, touchNormal;

        public FixationState(MovementInfo movement, Vector3 touchPosition, Vector3 touchNormal, DateTime touchedAt, StateSettings settings = default, bool isStateChanged = true)
            : base(movement, settings, isStateChanged)
        {
            this.touchPosition = touchPosition;
            this.touchNormal = touchNormal;
            this.touchedAt = touchedAt;
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, settings, true);
            else if ((createdAt - touchedAt).TotalSeconds >= settings.StandStillSeconds)
                return new LandedState(newMovement, settings, true);
            else if (Vector3.Distance(touchPosition, newMovement.Position) > settings.MaxMovementAfterTouch)
                return new SlippedState(newMovement, settings, true);
            else if (Vector3.Dot(touchNormal, newMovement.Normal) > settings.MaxRotationAfterTouch)
                return new CapsizedState(newMovement, settings, true);
            else
                return base.NextState(newMovement);
        }
    }
}