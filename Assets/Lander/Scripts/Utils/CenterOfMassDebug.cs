using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMassDebug : MonoBehaviour
{    
    public Rigidbody body;

	void Start()
	{
		body = GetComponent<Rigidbody>();
	}


    private void OnDrawGizmos()
    {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(body.worldCenterOfMass, 0.1f);
        Gizmos.DrawLine(body.worldCenterOfMass, body.worldCenterOfMass - 100*body.transform.up);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(body.transform.position, 0.02f);
    }
}

