using System.Linq;

using UnityEngine;

using Lander.Shared;


namespace Lander.Thrusters
{
    [DisallowMultipleComponent]
    public class Thruster : BaseThruster
    {
        private ParticleSystem[] exhausts;
        private AudioSource[] sounds;

        private Rigidbody body;
        private Vector3 thrustNormalized;
        private Color vectorColor = Color.gray;


        public override void Shutdown()
        {
            foreach (var e in exhausts.Where(e => e.isPlaying))
                e.Stop();
            foreach (var s in sounds.Where(s => s.isPlaying))
                s.Stop();

            thrustNormalized = Vector3.zero;
            this.thrust = 0;
        }

        public override void Burn(float thrust)
        {
            if (thrust <= 0)
                Shutdown();

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
            vectorColor = Color.red;
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