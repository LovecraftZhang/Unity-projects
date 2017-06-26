using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	public GameObject target;
	
	void LateUpdate() {
		// use bulidin LookAt method, create a stump for other cameras
		transform.LookAt(target.transform);
	}
}
