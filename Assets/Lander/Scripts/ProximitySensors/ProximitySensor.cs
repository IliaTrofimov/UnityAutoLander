using UnityEngine;
using Shared;

namespace ProximitySensors
{
	/// <summary>Сенсор, измеряющий расстояние вдоль оси Y в системе координат родительского объекта.</summary>
    public class ProximitySensor : BaseProximitySensor, ILabeled<PositionOnSpacecraft>
	{
        public PositionOnSpacecraft Position;

        public override float GetDistance()
		{
			direction = gameObject.transform.up;
			return base.GetDistance();
		}

        public PositionOnSpacecraft GetLabel()
        {
            return Position;
        }

        private void Start()
        {
            sensorLaserColor = Color.red;
        }
    }
}