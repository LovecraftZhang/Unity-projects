using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject target;
	public float damping = 100;
	Vector3 offset;
	
	void Start() {
		// initialize offset
		offset = target.transform.position - transform.position;
	}
	
	void LateUpdate() {
		float currentAngle = transform.eulerAngles.y;

		// gеt thе angle οf thе target
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
		
		// turn the angle іntο a rotation
		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		// rotate the camera
		transform.position = target.transform.position - (rotation * offset);
		
		transform.LookAt(target.transform);
	}
}