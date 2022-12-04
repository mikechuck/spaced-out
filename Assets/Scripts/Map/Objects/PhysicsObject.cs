using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 5f;
    private Vector3 _position;
	private Vector3 _down;
	protected Vector3 _velocity;
	protected Vector3 _gravity;
	protected GameObject _gameObject;

	public void ApplyGravity()
	{
		// get vector between current position and origin
		// find the normalized vector, multiply by gravityMagnitude
		Vector3 gravityVector = -_gameObject.transform.position.normalized * _gravityMagnitude;
		Debug.Log(gravityVector);

		if (_gameObject.transform.position)
		_gameObject.transform.position = _gameObject.transform.position + new Vector3(gravityVector.x * Time.deltaTime, gravityVector.y * Time.deltaTime, gravityVector.z * Time.deltaTime);
	}
}

// rotate player depending on position direction vector
// first just keep going in this direction...
// explore rigidbody instead ofcharacter controller (see if playercontroller can be used without character controller as well)
