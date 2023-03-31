using System;

using UnityEngine;

namespace CraftState
{
    public class FixationState : BaseState
    {
        private DateTime touchedAt;
        private Vector3 touchPosition, touchRotation;

        public FixationState(MovementInfo movement, Vector3 touchPosition, Vector3 touchRotation, DateTime touchedAt) : base(movement)
        {
            this.touchPosition = touchPosition;
            this.touchRotation = touchRotation;
            this.touchedAt = touchedAt;
        }

        public override BaseState NextState(MovementInfo newMovement)
        {
            if (!newMovement.IsCollided)
                return new FlyingState(newMovement);
            else if ((createdAt - touchedAt).TotalSeconds >= 2)
                return new LandedState(newMovement);
            else if (Vector3.Distance(touchPosition, newMovement.Position) > 5 || Vector3.Distance(touchRotation, newMovement.EulerAngles) > 20)
                return new CapsizedState(newMovement);
            else
                return new FixationState(newMovement, touchPosition, touchRotation, touchedAt);
        }
    }

}