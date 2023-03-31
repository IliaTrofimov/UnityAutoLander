using System;
using UnityEngine;


namespace CraftState
{
    public class MovementInfo
    {
        public bool IsCollided { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
    }
}