using System;
using UnityEngine;

using Shared;

namespace CraftState
{
    public class FixationState : BaseState
    {
        private DateTime touchedAt;
        private Vector3 touchPosition, touchNormal;

        public FixationState(MovementInfo movement, Vector3 touchPosition, Vector3 touchNormal, DateTime touchedAt, bool isStateChanged = true)
            : base(movement, isStateChanged)
        {
            this.touchPosition = touchPosition;
            this.touchNormal = touchNormal;
            this.touchedAt = touchedAt;
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement, true);
            else if ((createdAt - touchedAt).TotalSeconds >= StandStillSeconds)
                return new LandedState(newMovement, true);
            else if (Vector3.Distance(touchPosition, newMovement.Position) > MaxMovementAfterTouch)
                return new SlippedState(newMovement, true);
            else if (Vector3.Dot(touchNormal, newMovement.Normal) > MaxRotationAfterTouch)
                return new CapsizedState(newMovement, true);
            else
                return base.NextState(newMovement);
        }
    }
}