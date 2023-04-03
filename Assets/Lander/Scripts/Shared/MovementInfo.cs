using UnityEngine;


namespace Shared
{
    public class MovementInfo
    {
        private Vector3 eulerAngles;
        public bool IsCollided { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Quaternion { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public Vector3 Normal { get; set; }

        public override string ToString()
        {
            var euler = Quaternion.eulerAngles;
            return $"Pos=({Position.x:F1},{Position.y:F1},{Position.z:F1}) Rot=({euler.x:F1},{euler.y:F1},{euler.z:F1}) Spd={Velocity.magnitude:F1}{(IsCollided ? " COLLIDED" : "")}";
        }
    }
}