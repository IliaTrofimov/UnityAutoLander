using System.Linq;

using UnityEngine;

using Lander.Shared;


namespace Lander.Thrusters
{

    [DisallowMultipleComponent]
    public class ThrusterMK2 : BaseThruster
    {
        [SerializeField]
        private float maxThrustValue;

        [SerializeField]
        public PositionOnSpacecraft Position;

        private float thrust;
        private ParticleSystem[] exhausts;
        private AudioSource[] sounds;

        private Rigidbody body;
        private Vector3 thrustNormalized;
        private Color vectorColor = Color.gray;

        public override float MaxThrustValue => maxThrustValue;
        public override float Thrust => thrust;


        /// <summary>Выключение двигателя. Выключение всех эффектов и звуков.</summary>
        public override void Shutdown()
        {
            foreach (var e in exhausts.Where(e => e.isPlaying))
                e.Stop();
            foreach (var s in sounds.Where(s => s.isPlaying))
                s.Stop();

            thrustNormalized = Vector3.zero;
            this.thrust = 0;
        }

        /// <summary>Включение двигателя и применение тяги. Включение всех эффектов и звуков.</summary>
        public override void Burn(float thrust)
        {
            foreach (var e in exhausts.Where(e => !e.isPlaying))
                e.Play();
            foreach (var s in sounds.Where(s => !s.isPlaying))
                s.Play();

            var pos = gameObject.transform.position;
            this.thrust = thrust * MaxThrustValue;
            var force = -transform.up * this.thrust;
            body.AddForceAtPosition(force, pos, ForceMode.Impulse);
            thrustNormalized = force.normalized;
        }


        private void Start()
        {
            body = gameObject.GetComponentInParent<Rigidbody>();
            exhausts = gameObject.GetComponentsInChildren<ParticleSystem>();
            sounds = gameObject.GetComponentsInChildren<AudioSource>();
            vectorColor = AxisInfo.GetColorForAxis(Position.Axis);
        }

        private void OnDrawGizmos()
        {
            if (thrustNormalized.x != 0 || thrustNormalized.y != 0 || thrustNormalized.z != 0)
            {
                Gizmos.color = vectorColor;
                Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
                Gizmos.DrawRay(gameObject.transform.position, thrustNormalized * 2);
            }
        }
    }
}