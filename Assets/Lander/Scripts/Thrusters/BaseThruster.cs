using UnityEngine;

namespace Lander.Thrusters
{
    public abstract class BaseThruster : MonoBehaviour
    {
        public abstract float MaxThrustValue { get; }
        public abstract float Thrust { get; }

        public abstract void Burn(float thrust);
        public abstract void Shutdown();
    }
}