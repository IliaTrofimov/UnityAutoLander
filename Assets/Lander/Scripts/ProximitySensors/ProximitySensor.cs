using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ProximitySensors
{
	public class ProximitySensorEvent
	{
		public string Uid { get; set; }
		public float Distance { get; set; }

		public ProximitySensorEvent(string uid, float dist)
		{
			Uid = uid;
			Distance = dist;
		}
	}


    public class ProximitySensor : MonoBehaviour, IProximitySensor
	{
		public UnityEvent<ProximitySensorEvent> OnMessureDistance;
        public string Uid => gameObject.name;

		[Range(0.0001f, 100.0f)]
		public float DangerDistance = 10.0f;

        [Range(10.0f, 10000.0f)]
        public float MaxDistance = 100.0f;

        public float Distance => distance;

		private float distance = float.PositiveInfinity;
        private LineRenderer lineRenderer;
		private Vector3 hitPosition;
		private Vector3 position;


        private void Start()
		{
			lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        private void FixedUpdate()
        {
			GetDistance();
			OnMessureDistance.Invoke(new ProximitySensorEvent(Uid, distance));
        }

        private void Update()
        {
            lineRenderer.SetPositions(new Vector3[] { position, hitPosition });
			lineRenderer.endColor = distance <= DangerDistance ? Color.red : Color.white;
        }


        public float GetDistance()
		{
			position = gameObject.transform.position;
			var direction = gameObject.transform.up;
			
			if (Physics.Raycast(new Ray(position, direction), out RaycastHit hit, MaxDistance, ~Physics.IgnoreRaycastLayer))
			{
				distance = hit.distance;
				hitPosition = hit.point;
                lineRenderer.enabled = true;
            }
			else
			{
				distance = float.PositiveInfinity;
				lineRenderer.enabled = false;
			}
			return distance;
		}
    }

	public class SensorsInfo
	{
		public float Height { get; set; }
		public Dictionary<string, float> Proximity { get; set; } = new();
	}
}