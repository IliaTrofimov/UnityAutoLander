using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Shared;


namespace Thrusters
{
    /// <summary>Одиночный двигатель космического корабля.</summary>
    [DisallowMultipleComponent]
    public class Thruster : MonoBehaviour, ILabeledWithOrder<PositionOnSpacecraft>
    {
        private ParticleSystem[] exhausts;
        private AudioSource[] sounds;
        
        private Rigidbody body;
        private Vector3 thrustNormalized;
        private Color vectorColor = Color.gray;

        [Range(0, 100)]
        public float MaxThrustValue = 1.0f;

        [SerializeField]
        public PositionOnSpacecraft Position;
        public int Order;


        public (PositionOnSpacecraft, int) GetLabel()
        {
            return (Position, Order);
        }

        /// <summary>Выключение двигателя. Выключение всех эффектов и звуков.</summary>
        public void Shutdown()
        {
            foreach (var e in exhausts.Where(e => e.isPlaying))
                e.Stop();
            foreach (var s in sounds.Where(s => s.isPlaying))
                s.Stop();

            thrustNormalized = Vector3.zero;
        }

        /// <summary>Включение двигателя и применение тяги. Включение всех эффектов и звуков.</summary>
        public void Burn(float thrust)
        {
            foreach (var e in exhausts.Where(e => !e.isPlaying))
                e.Play();
            foreach (var s in sounds.Where(s => !s.isPlaying))
                s.Play();

            var pos = gameObject.transform.position;
            var force = GetThrustForce() * thrust * MaxThrustValue;
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
            if (thrustNormalized.x != 0 && thrustNormalized.y != 0 && thrustNormalized.z != 0)
            {
                Gizmos.color = vectorColor;
                Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
                Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + thrustNormalized * 2);
            }
        }

        private Vector3 GetThrustForce()
        {
            switch (Position.Axis)
            {
                case AxisInfo.Axis.X:
                    return body.transform.right * (int)Position.Direction;
                case AxisInfo.Axis.Y:
                    return body.transform.up * (int)Position.Direction;
                case AxisInfo.Axis.Z:
                    return body.transform.forward * (int)Position.Direction;
                default:
                    return Vector3.zero;
            }
        }
    }
}