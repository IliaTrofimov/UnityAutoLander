using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

using Thrusters;
using Utils;
using CraftState;

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

        private ThrustersController thrustersController;
        private BaseState state;
        private Rigidbody body;
        private bool isCollided;

        private void Start()
        {
            body = gameObject.GetComponent<Rigidbody>();
            thrustersController = new ThrustersController(GetComponentsInChildren<Thruster>());
            state = new FlyingState(new MovementInfo() { });
        }

        private void Update()
        {
            if (DrawVelocityVector)
                Debug.DrawLine(body.position, body.position + body.velocity.normalized*5, Color.yellow);
        }


        private void FixedUpdate()
        {
            float pitch = Input.GetAxis("Horizontal") * RollThrust;
            float roll = Input.GetAxis("Vertical") * PitchThrust;
            float yaw = Input.GetAxis("Yaw") * YawThrust;
            float up = Input.GetAxis("Jump") * MainThrust;

            thrustersController.ApplyMovement(0, up, 0, roll, 0, pitch);

            var temp = state.NextState(GetMovementInfo());
            if (temp.GetType() != state.GetType())
                Debug.Log($"{state} -> {temp}");
            state = temp;

            if (Input.GetKey(KeyCode.LeftShift))
                body.angularVelocity = new Vector3();
            if (Input.GetKey(KeyCode.LeftControl))
                body.velocity = new Vector3();
        }

        private void OnCollisionEnter(Collision collision)
        {
            isCollided = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isCollided = false;
        }



        private MovementInfo GetMovementInfo()
        {
            return new MovementInfo()
            {
                Position = body.worldCenterOfMass,
                Velocity = body.velocity,
                EulerAngles = body.transform.eulerAngles,
                AngularVelocity = body.angularVelocity,
                Height = 0,
                IsCollided = isCollided
            };
        }
    }

}