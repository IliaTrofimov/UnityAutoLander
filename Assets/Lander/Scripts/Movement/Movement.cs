using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using Shared;
using Thrusters;
using CraftState;

namespace Movement
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ThrustersController), typeof(Rigidbody))]
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

        private BaseState state;
        private bool isCollided;
        private ThrustersController thrustersController;
        private Rigidbody body;

        private void Start()
        {
            body = gameObject.GetComponent<Rigidbody>();
            thrustersController = gameObject.GetComponent<ThrustersController>();
        }


        private void FixedUpdate()
        {
            float pitch = Input.GetAxis("Horizontal") * RollThrust;
            float roll = Input.GetAxis("Vertical") * PitchThrust;
            float yaw = Input.GetAxis("Yaw") * YawThrust;
            float up = Input.GetAxis("Jump") * MainThrust;

            thrustersController.ApplyMovement(0, up, 0, roll, yaw, pitch);

            if (Input.GetKey(KeyCode.LeftShift))
                body.angularVelocity = new Vector3();
            if (Input.GetKey(KeyCode.LeftControl))
                body.velocity = new Vector3();
        }
    }
}