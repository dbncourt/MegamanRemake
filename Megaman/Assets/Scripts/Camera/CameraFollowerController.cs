using System.Collections;
using System.Collections.Generic;
using Project.Constants;
using UnityEngine;

public class CameraFollowerController : MonoBehaviour
{
	[SerializeField]
	private Transform characterToFollow;

	void Start ()
	{
		if (characterToFollow == null) {
			Debug.LogError ("Character To Follow is not Set");
		}
	}

	void Update ()
	{
		Vector3 position = new Vector3 (characterToFollow.position.x, transform.position.y, transform.position.z);
		transform.SetPositionAndRotation (position, transform.rotation);
	}
}