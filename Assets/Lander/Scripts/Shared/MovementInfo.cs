using UnityEngine;


namespace Shared
{
    public class MovementInfo
    {
        public bool IsCollided { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        public override string ToString()
        {
            return $"Pos:{Position.x:F2},{Position.y:F2},{Position.z:F2} Rot:{EulerAngles.x:F2},{EulerAngles.y:F2},{EulerAngles.z:F2} Vel:{Velocity.magnitude:F1}{(IsCollided ? " COLLIDED" : "")}";
        }
    }
}