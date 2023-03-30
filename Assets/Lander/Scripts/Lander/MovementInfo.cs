using UnityEngine;

namespace Lander
{
    public class MovementInfo
    {
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        public MovementInfo(Vector3 position, Vector3 eulerAngles, Vector3 velocity, Vector3 angularVelocity)
        {
            Position = position;
            EulerAngles = eulerAngles;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }
    }

}