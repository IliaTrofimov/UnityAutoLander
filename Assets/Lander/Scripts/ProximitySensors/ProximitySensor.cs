using UnityEngine;
using Lander.Shared;

namespace Lander.ProximitySensors
{
	/// <summary>Сенсор, измеряющий расстояние вдоль оси Y в системе координат родительского объекта.</summary>
    public class ProximitySensor : BaseProximitySensor
	{

        [SerializeField]
        private int order;

        public override float GetDistance()
		{
			direction = gameObject.transform.up;
			return base.GetDistance();
		}

        private void Start()
        {
            sensorLaserColor = Color.red;
        }
    }
}