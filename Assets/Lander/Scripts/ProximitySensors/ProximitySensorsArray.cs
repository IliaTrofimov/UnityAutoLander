using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Lander.Shared;

namespace Lander.ProximitySensors
{
    public class ProximitySensorsArray : MonoBehaviour
    {
        [SerializeField]
        private bool LogSensorData;

        [SerializeField]
        public List<ProximitySensor> Sensors;
        


        private void FixedUpdate()
        {
            
        }
    }
}