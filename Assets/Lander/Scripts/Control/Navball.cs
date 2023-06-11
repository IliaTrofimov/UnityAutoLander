using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lander.Control
{
    [DisallowMultipleComponent]
    public class Navball : MonoBehaviour
    {
        [SerializeField]
        private GameObject VelocityMarker;

        [SerializeField]
        private GameObject Ball;

        [SerializeField]
        private Rigidbody Rigidbody;

        [SerializeField]
        private Slider SliderX;

        [SerializeField]
        private Slider SliderY;

        [SerializeField]
        private Slider SliderZ;

        private bool hasSliders;


        private void Start()
        {
            var sliders = gameObject.GetComponentsInChildren<Slider>();
            SliderX = sliders.FirstOrDefault(s => s.name == "Slider X");
            SliderY = sliders.FirstOrDefault(s => s.name == "Slider Y");
            SliderZ = sliders.FirstOrDefault(s => s.name == "Slider Z");
            hasSliders = SliderX != null && SliderY != null && SliderZ != null;
        }

        private void Update()
        {   
            Ball.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            var normVelocity = Rigidbody.velocity.normalized;
            VelocityMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, Rigidbody.velocity.normalized);

            if (hasSliders)
            {
                SliderX.value = Mathf.Atan(Rigidbody.angularVelocity.x);
                SliderY.value = Mathf.Atan(Rigidbody.angularVelocity.y);
                SliderZ.value = Mathf.Atan(Rigidbody.angularVelocity.z);
            }
        }
    }
}