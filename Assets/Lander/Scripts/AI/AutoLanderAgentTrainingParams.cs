using UnityEngine;

namespace Lander.AI
{

    [System.Serializable]
    [CreateAssetMenu()]
    public class AutoLanderAgentTrainingParams : ScriptableObject
    {
        [Header("Velocity")]
        public Vector3 InitialVelocity;
        [Range(0f, 100f)]
        public float VelocityDispersion;

        [Space()]
        public Vector3 InitialAngularVelocity;
        [Range(0f, 5f)]
        public float AngularVelocityDispersion;

        [Header("Position")]
        [Range(0f, 100f)]
        public float PositionDispersion;

        [Range(0f, 30f)]
        public float RotationDispersion;

        [Header("Reset options")]
        public bool ResetAfterEpisode;

        [Range(0f, 10000f)]
        public float MaxSpeed;

        [Range(0f, 10000f)]
        public float MaxHorizontalDistance;

        [Range(0f, 10000f)]
        public float MaxVerticalDistance;

        [Range(10, 300)]
        public int MaxEpisodeSeconds = 60;


        public bool ShouldReset(Rigidbody body) =>
            (MaxHorizontalDistance != 0 &&
                (MaxHorizontalDistance - Mathf.Abs(body.position.x) < 0) ||
                (MaxHorizontalDistance - Mathf.Abs(body.position.z) < 0)
            ) ||
            (MaxVerticalDistance != 0 && MaxVerticalDistance - Mathf.Abs(body.position.y) < 0) ||
            (MaxSpeed != 0 && body.velocity.magnitude > MaxSpeed);
    }
}