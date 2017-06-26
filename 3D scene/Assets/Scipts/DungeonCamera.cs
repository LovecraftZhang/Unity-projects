using UnityEngine;
using System.Collections;

public class DungeonCamera : MonoBehaviour {
	public GameObject target;
	public float damping = 1;
	Vector3 offset;

	void Start() {
		// initrialize the offset
		offset = transform.position - target.transform.position;
	}
	
	void LateUpdate() {
		// update the position of camera
		Vector3 desiredPosition = target.transform.position + offset;
		// make the motion smooth
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
		
		transform.position = position;

		transform.LookAt(target.transform.position);
	}
}
