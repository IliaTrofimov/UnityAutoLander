using UnityEngine;

namespace Lander.CraftState
{
    [System.Serializable]
    [CreateAssetMenu()]
    public class StateSettings : ScriptableObject
    {
        public float MaxMovementAfterTouch = 5;
        public float MaxRotationAfterTouch = 20;
        public float StandStillSeconds = 2;
        public float MaxTocuhVelocity = 40;
    }
}