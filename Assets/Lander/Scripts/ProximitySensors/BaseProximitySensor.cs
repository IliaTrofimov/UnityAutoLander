using UnityEngine;
using UnityEngine.Events;

namespace ProximitySensors
{
    /// <summary>Базовый класс для всех сенсоров расстояния.</summary>
    public abstract class BaseProximitySensor : MonoBehaviour, IProximitySensor
    {
        protected float distance = float.PositiveInfinity;
        protected Vector3 direction;
        protected Vector3 hitPosition;

        protected Color sensorLaserColor;

        [Range(0.0001f, 100.0f)]
        public float DangerDistance = 10.0f;

        [Range(10.0f, 10000.0f)]
        public float MaxDistance = 100.0f;

        public float Distance => distance;


        public virtual float GetDistance()
        {
            var position = gameObject.transform.position;

            if (Physics.Raycast(new Ray(position, direction), out RaycastHit hit, MaxDistance, ~Physics.IgnoreRaycastLayer))
            {
                distance = hit.distance;
                hitPosition = hit.point;
            }
            else
            {
                distance = float.PositiveInfinity;
            }
            return distance;
        }


        protected virtual void OnDrawGizmos()
        {
            if (float.IsFinite(distance))
            {
                Gizmos.color = distance > DangerDistance ? Color.gray : sensorLaserColor;
                Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
                Gizmos.DrawLine(gameObject.transform.position, hitPosition);
            }
        }

        protected virtual void FixedUpdate()
        {
            GetDistance();
        }
    }
}