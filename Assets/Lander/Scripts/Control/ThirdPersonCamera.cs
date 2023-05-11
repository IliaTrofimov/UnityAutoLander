using UnityEngine;


namespace Lander.Control
{
    [DisallowMultipleComponent]
    public class ThirdPersonCamera : MonoBehaviour
    {
        public float turnSpeed = 4.0f;
        public GameObject target;
        private float targetDistance;
        public float minTurnAngle = -90.0f;
        public float maxTurnAngle = 0.0f;
        private float rotX;
        private new Camera camera;

        void Start()
        {
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            camera = GetComponent<Camera>();
        }

        void Update()
        {
            float y = Input.GetAxis("Mouse X") * turnSpeed;
            rotX += Input.GetAxis("Mouse Y") * turnSpeed;
            rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

            transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
            transform.position = target.transform.position - (transform.forward * targetDistance);
        }
    }
}