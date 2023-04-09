using System.Collections.Generic;

using UnityEngine;

using Lander.Shared;

namespace Lander.ProximitySensors
{
    public class ProximitySensorsArray : MonoBehaviour
    {
        public Dictionary<PositionOnSpacecraft, List<ProximitySensor>> Sensors { get; private set; } = new();

        private void Start()
        {
            foreach (var s in GetComponentsInChildren<ProximitySensor>())
            {
                (PositionOnSpacecraft pos, int order) = s.GetLabel();
                if (!Sensors.TryAdd(pos, new List<ProximitySensor>() { s }))
                    Sensors[pos].Add(s);
            }

            foreach (var s in Sensors.Values)
                s.Sort((ProximitySensor a, ProximitySensor b) => a.GetLabel().order.CompareTo(b.GetLabel().order));
        }
    }
}