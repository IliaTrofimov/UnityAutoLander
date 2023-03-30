using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Utils;

namespace Thrusters
{

    public class Thruster : MonoBehaviour
    {
        private ParticleSystem[] exhausts;
        private AudioSource[] sounds;
        
        private ThrustersController controller;
        private string thrusterName;
        private Vector3 thrustNormalized;
        private Color vectorColor = Color.gray;

        public float BaseThrustValue = 1.0f;
        public AxisInfo.Axis Axis;
        public AxisInfo.Direction Direction;
        public ThrusterPlacement Placement;
        public bool DrawDebugLines;
        public string ThrusterName => thrusterName;

        public UnityEvent<ThrustInfo> ThrusterBurnEvent;


        private void Start()
        {
            exhausts = gameObject.GetComponentsInChildren<ParticleSystem>();
            controller = gameObject.GetComponentInParent<ThrustersController>();
            sounds = gameObject.GetComponentsInChildren<AudioSource>();
            thrusterName = gameObject.name;
            vectorColor = AxisInfo.GetColorForAxis(Axis);
        }

        private void Update()
        {
            if (DrawDebugLines && thrustNormalized != Vector3.zero)
               Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + thrustNormalized.normalized * 3, vectorColor);
        }

        public void Shutdown()
        {
            foreach (var e in exhausts.Where(e => e.isPlaying))
                e.Stop();
            foreach (var s in sounds.Where(s => s.isPlaying))
                s.Stop();

            thrustNormalized = Vector3.zero;
        }

        public void Burn(float thrust)
        {
            foreach (var e in exhausts.Where(e => !e.isPlaying))
                e.Play();
            foreach (var s in sounds.Where(s => !s.isPlaying))
                s.Play();

            var pos = gameObject.transform.position;
            var force = GetThrustForce() * thrust * BaseThrustValue;
            controller.Body.AddForceAtPosition(force, pos, ForceMode.Impulse);
            thrustNormalized = force.normalized;

            ThrusterBurnEvent.Invoke(new ThrustInfo(Axis, Direction, Placement, thrust));
        }

        private Vector3 GetThrustForce()
        {
            switch (Axis)
            {
                case AxisInfo.Axis.X:
                    return gameObject.transform.right * (int)Direction;
                case AxisInfo.Axis.Y:
                    return gameObject.transform.up * (int)Direction;
                case AxisInfo.Axis.Z:
                    return gameObject.transform.forward * (int)Direction;
                default:
                    return Vector3.zero;
            }
        }

 
    }


}