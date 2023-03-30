using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class RolloverDetection : MonoBehaviour
{
    [Range(0.1f, 50)]
    public float MaxCollisionVelocity = 10;

	[Range(0, 360)]
	public float MaxAngleMovement = 20;

    [Range(0, 5)]
    public float MaxMovement = 0.2f;

    [Range(0.01f, 5)]
    public float StandingStillSeconds = 2;

	private bool isCrashed = false;
    private bool isFirstTouch = true;
	private Vector3? landingRotation = null;
	private Vector3? landingPosition = null;
	private DateTime? landingTime = null;
	private Rigidbody body;


	void Start()
	{
		body = GetComponent<Rigidbody>();
    }

	
	void FixedUpdate()
	{
		if (!isFirstTouch && !isCrashed && (DateTime.Now - landingTime.Value).TotalSeconds >= StandingStillSeconds)
		{
			Reset();
            Debug.Log("Finally landed!");
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
		if (isFirstTouch)
		{
			if (collision.relativeVelocity.magnitude >= MaxCollisionVelocity)
				Debug.Log("Crash!");
			else
				Debug.Log("Touch");

			landingTime = DateTime.Now;
			landingRotation = body.transform.eulerAngles;
			landingPosition = body.position;
			isFirstTouch = false;
			isCrashed = false;
		}
		else if ((DateTime.Now - landingTime.Value).TotalSeconds < StandingStillSeconds)
		{
			if (Vector3.Distance(body.transform.eulerAngles, landingRotation.Value) >= MaxAngleMovement)
			{
				Debug.Log("Too much rotation after landing!");
				isCrashed = true;
			}
			if (Vector3.Distance(body.transform.position, landingPosition.Value) >= MaxMovement)
			{
				Debug.Log("Too much movement after landing!");
                isCrashed = true;
            }

			if (isCrashed) Reset();
		}
    }

    private void Reset()
    {
        isFirstTouch = true;
        landingTime = null;
        landingRotation = null;
        landingPosition = null;
		isCrashed = false;
    }
}

