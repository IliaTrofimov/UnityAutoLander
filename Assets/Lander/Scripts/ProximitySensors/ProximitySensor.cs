using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace ProximitySensors
{
    public abstract class BaseProximitySensor : MonoBehaviour, IProximitySensor
    {
        public UnityEvent<ProximitySensorEvent> OnMessureDistance;
        public string Uid => gameObject.name;

        public float Distance => distance;

        [Range(0.0001f, 100.0f)]
        public float DangerDistance = 10.0f;

        [Range(10.0f, 10000.0f)]
        public float MaxDistance = 100.0f;

        protected float distance = float.PositiveInfinity;
        protected LineRenderer lineRenderer;
        protected Vector3 hitPosition;


        protected void Start()
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        protected void FixedUpdate()
        {
            GetDistance();
        }

        private void OnDrawGizmos()
        {
            if (float.IsFinite(distance))
            {
                Gizmos.color = distance <= DangerDistance ? Color.red : Color.white;
                Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
                Gizmos.DrawLine(gameObject.transform.position, hitPosition);
            }
        }


        protected void Update()
        {
            if (float.IsFinite(distance))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[] { gameObject.transform.position, hitPosition });
                lineRenderer.endColor = distance <= DangerDistance ? Color.blue : Color.white;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }

        public abstract float GetDistance();
    }

    public class ProximitySensor : BaseProximitySensor
	{
        public override float GetDistance()
		{
			var direction = gameObject.transform.up;
			
			if (Physics.Raycast(new Ray(gameObject.transform.position, direction), out RaycastHit hit, MaxDistance, ~Physics.IgnoreRaycastLayer))
			{
				distance = hit.distance;
				hitPosition = hit.point;
            }
			else
			{
				distance = float.PositiveInfinity;
			}
            OnMessureDistance.Invoke(new ProximitySensorEvent(Uid, distance));
            return distance;
		}
    }
}