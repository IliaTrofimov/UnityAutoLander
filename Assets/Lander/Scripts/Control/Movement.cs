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
        [Range(1, 50)]
        [SerializeField]
        private float PitchThrust = 5;

        [Range(1, 50)]
        [SerializeField]
        private float RollThrust = 5;

        [Range(1, 50)]
        [SerializeField]
        private float YawThrust = 5;

        [Range(1, 250)]
        [SerializeField]
        private float MainThrust = 100;

        [SerializeField]
        private ThrustersController thrustersController;



        private void FixedUpdate()
        {
            float pitch = Input.GetAxis("Roll") * RollThrust;
            float roll = Input.GetAxis("Pitch") * PitchThrust;
            float yaw = Input.GetAxis("Yaw") * YawThrust;
            float up = Input.GetAxis("Jump") * MainThrust;
           
            if (Input.GetKeyDown(KeyCode.LeftShift))
                thrustersController.ApplyMovement(roll, up, pitch, 0, yaw, 0);
            else
                thrustersController.ApplyMovement(0, up, 0, roll, yaw, pitch);
        }
    }
}