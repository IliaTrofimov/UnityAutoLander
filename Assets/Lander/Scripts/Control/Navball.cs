using UnityEngine;


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


        private void Update()
        {
            Ball.transform.rotation = Quaternion.FromToRotation(Rigidbody.transform.forward, Rigidbody.transform.up);
            var normVelocity = Rigidbody.velocity.normalized;
            VelocityMarker.transform.rotation = Quaternion.FromToRotation(Vector3.up, Rigidbody.velocity.normalized);
        }
    }
}