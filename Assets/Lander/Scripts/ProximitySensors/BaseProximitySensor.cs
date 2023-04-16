using System;

using UnityEngine;
using UnityEngine.Events;
using Lander.Thrusters;
using GlobalShared;

namespace Lander.ProximitySensors
{

    /// <summary>Базовый класс для всех сенсоров расстояния.</summary>
    public abstract class BaseProximitySensor : MonoBehaviour
    {
        protected float distance = -1;
        protected Vector3 direction;
        protected Vector3 hitPosition;

        protected Color sensorLaserColor;

        [Range(0.0001f, 100.0f)]
        [SerializeField]
        private float dangerDistance = 10.0f;

        [Range(10.0f, 10000.0f)]
        [SerializeField]
        private float maxDistance = 100.0f;

        public float Distance => distance;
        public float DangerDistance => dangerDistance;
        public float MaxDistance => maxDistance;


        public virtual float GetDistance()
        {
            var position = gameObject.transform.position;

            if (Physics.Raycast(new Ray(position, direction), out RaycastHit hit, maxDistance, ~Physics.IgnoreRaycastLayer))
            {
                distance = hit.distance;
                hitPosition = hit.point;
            }
            else
            {
                distance = -1;
            }
            return distance;
        }


        protected virtual void OnDrawGizmos()
        {
            if (distance > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.color = distance > dangerDistance ? Color.gray : sensorLaserColor;
                Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
                Gizmos.DrawLine(gameObject.transform.position, hitPosition);
                Gizmos.DrawSphere(hitPosition, 0.1f);
            }
        }

        protected virtual void FixedUpdate()
        {
            GetDistance();
        }
    }
}