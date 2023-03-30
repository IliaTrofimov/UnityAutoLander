using System;

namespace Thrusters
{
    public interface IThrustersController
    {
        /// <param name="moveX">Parallel movement along X</param>
        /// <param name="moveY">Parallel movement along Y</param>
        /// <param name="moveZ">Parallel movement along Z</param>
        /// <param name="rotX">Rotation along X (pitch, nose up/down)</param>
        /// <param name="rotY">Rotation along Y (yaw, nose left/right)</param>
        /// <param name="rotZ">Rotation along Z (roll)</param>
        public void ApplyMovement(float moveX, float moveY, float moveZ, float rotX, float rotY, float rotZ);
    }

}