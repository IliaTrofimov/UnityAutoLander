using UnityEngine;


namespace Lander.Shared
{
    public class MovementInfo
    {
        public bool IsCollided { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; } = new();
        public Vector3 Velocity { get; set; } = new();
        public Vector3 AngularVelocity { get; set; } = new();
        public Vector3 Normal { get; set; } = new();

        public override string ToString()
        {
            return $"Pos=({Position.x:F1},{Position.y:F1},{Position.z:F1}) Nrm=({Normal.x:F1},{Normal.y:F1},{Normal.z:F1}) H={Height:F1} Spd={Velocity.magnitude:F1}{(IsCollided ? " COLLIDED" : "")}";
        }
    }
}