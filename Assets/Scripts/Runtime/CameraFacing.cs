using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{
	[SerializeField] Camera _cameraToLookAt;

	private void Awake()
	{
		if(_cameraToLookAt == null) _cameraToLookAt = Camera.main; 
	}
	void Update() 
	{
		Vector3 delta = _cameraToLookAt.transform.position - transform.position;
		delta.x = delta.z = 0.0f;
		transform.LookAt(_cameraToLookAt.transform.position - delta); 
	}
}