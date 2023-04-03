using System;
using UnityEngine;

using Shared;

namespace CraftState
{
    public class FixationState : BaseState
    {
        public static float MaxMovementAfterTouch = 5;
        public static float MaxRotationAfterTouch = 20;
        public static float StandStillSeconds = 2;

        private DateTime touchedAt;
        private Vector3 touchPosition, touchRotation;

        public FixationState(MovementInfo movement, Vector3 touchPosition, Vector3 touchRotation, DateTime touchedAt, bool isStateChanged = false)
            : base(movement, isStateChanged)
        {
            this.touchPosition = touchPosition;
            this.touchRotation = touchRotation;
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
            else if (Vector3.Distance(touchRotation, newMovement.EulerAngles) > MaxRotationAfterTouch)
                return new CapsizedState(newMovement, true);
            else
                return base.NextState(newMovement);
        }
    }
}