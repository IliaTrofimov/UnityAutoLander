using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

using Thrusters;

namespace Lander
{

    public class Movement : MonoBehaviour
    {
        [Range(1, 50)]
        public float PitchThrust = 5;

        [Range(1, 50)]
        public float RollThrust = 5;

        [Range(1, 50)]
        public float YawThrust = 5;

        [Range(1, 250)]
        public float MainThrust = 100;

        public bool DrawVelocityVector = true;
        public bool DrawCenterOfMass = true;

        public UnityEvent<MovementInfo> ObjectMovedEvent;
        public ThrustersController thrustersController;
        private Rigidbody body;

        void Start()
        {
            body = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (DrawVelocityVector)
                Debug.DrawLine(body.position, body.position + body.velocity.normalized*5, Color.yellow);
        }



        void FixedUpdate()
        {
            float pitch = Input.GetAxis("Roll") * RollThrust;
            float roll = Input.GetAxis("Pitch") * PitchThrust;
            float yaw = Input.GetAxis("Yaw") * YawThrust;
            float up = Input.GetAxis("Thrust") * MainThrust;

            thrustersController.ApplyMovement(0, up, 0, roll, yaw, pitch);
            ObjectMovedEvent.Invoke(new MovementInfo(body.position, body.transform.eulerAngles, body.velocity, body.angularVelocity));

            if (Input.GetKey(KeyCode.LeftShift))
                body.angularVelocity = new Vector3();
            if (Input.GetKey(KeyCode.LeftControl))
                body.velocity = new Vector3();
        }
    }

}