using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

using Lander.Shared;
using Lander.Thrusters;
using Lander.CraftState;

namespace Lander.Control
{
    [DisallowMultipleComponent]
    public class Movement : MonoBehaviour
    {
        [Range(0, 50)]
        [SerializeField]
        private float PitchThrust = 5;

        [Range(0, 50)]
        [SerializeField]
        private float RollThrust = 5;

        [Range(0, 50)]
        [SerializeField]
        private float YawThrust = 5;

        [Range(0, 250)]
        [SerializeField]
        private float MainThrust = 100;

        [Range(0, 50)]
        [SerializeField]
        private float XThrust = 5;

        [Range(0, 50)]
        [SerializeField]
        private float ZThrust = 5;

        [SerializeField]
        private BaseThrustersController thrustersController;


        [SerializeField]
        private Rigidbody Rigidbody;

       

        private void FixedUpdate()
        {
            float pitch = Input.GetAxis("Roll") * RollThrust;
            float roll = Input.GetAxis("Pitch") * PitchThrust;
            float yaw = Input.GetAxis("Yaw") * YawThrust;

            float y = Input.GetAxis("Jump") * MainThrust;
            float x = Input.GetAxis("Horizontal") * XThrust;
            float z = Input.GetAxis("Vertical") * ZThrust;

            if (Input.GetKey(KeyCode.LeftShift))
                Rigidbody.angularVelocity = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftControl))
                Rigidbody.velocity = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Rigidbody.angularVelocity = Vector3.zero;
                Rigidbody.transform.rotation = new Quaternion();
            }

            thrustersController.ApplyMovement(x, y, z, roll, yaw, pitch);
        }
    }
}