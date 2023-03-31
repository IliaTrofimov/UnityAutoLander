using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

using Utils;

namespace ProximitySensors
{

    public class FixedProximitySensor : MonoBehaviour, IProximitySensor
	{
        public string Uid => gameObject.name;

        [Range(0.0001f, 100.0f)]
		public float DangerDistance = 10.0f;

        [Range(10.0f, 10000.0f)]
        public float MaxDistance = 100.0f;

        public float Distance => distance;
        public AxisInfo.Axis Axis = AxisInfo.Axis.Y;
        public AxisInfo.Direction Direction = AxisInfo.Direction.Negative;
        public UnityEvent<ProximitySensorEvent> OnMessureDistance;

        private Vector3 hitPosition;
		private float distance = float.PositiveInfinity;
		private Vector3 direction;
        private LineRenderer lineRenderer;


        private void Start()
		{
			lineRenderer = GetComponentInChildren<LineRenderer>();
			direction = AxisInfo.GetVector(Axis, Direction);
        }

        private void FixedUpdate()
        {
			GetDistance();
        }

        private void OnDrawGizmos()
        {
            if (float.IsFinite(distance))
            {
                Gizmos.color = distance <= DangerDistance ? Color.blue : Color.white;
                Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
                Gizmos.DrawLine(gameObject.transform.position, hitPosition);
            }
        }

        private void Update()
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


        public float GetDistance()
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
            OnMessureDistance.Invoke(new ProximitySensorEvent(Uid, distance));
            return distance;
		}
    }
}