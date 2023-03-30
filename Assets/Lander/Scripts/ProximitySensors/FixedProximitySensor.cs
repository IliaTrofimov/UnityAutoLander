﻿using UnityEngine;
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

		private float distance = float.PositiveInfinity;
        private LineRenderer lineRenderer;
		private Vector3 direction;

        private void Start()
		{
			lineRenderer = GetComponentInChildren<LineRenderer>();
			direction = AxisInfo.GetVector(Axis, Direction);
        }

        private void FixedUpdate()
        {
			GetDistance();
        }

        private void Update()
        {
			GetDistance();
        }


        public float GetDistance()
		{
			var position = gameObject.transform.position;
			
			if (Physics.Raycast(new Ray(position, direction), out RaycastHit hit, MaxDistance, ~Physics.IgnoreRaycastLayer))
			{
				distance = hit.distance;
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[] { position, hit.point });

				if (distance <= DangerDistance)
					lineRenderer.endColor = Color.blue;
				else
                    lineRenderer.endColor = Color.white;
            }
			else
			{
				distance = float.PositiveInfinity;
				lineRenderer.enabled = false;
			}
			return distance;
		}
    }
}