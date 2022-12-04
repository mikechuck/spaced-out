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

	public void ApplyPhysics()
	{
		_down = -_gameObject.transform.position.normalized;
		RotateObject();
		ApplyGravity();
	}

	private void RotateObject()
	{
		Debug.Log(_down);
		_gameObject.transform.rotation = Quaternion.Euler(_down.x, _down.y, _down.z);;
	}

	private void ApplyGravity()
	{
		Vector3 currentPosition = _gameObject.transform.position;
		Vector3 gravityVector = _down * _gravityMagnitude;
		_gameObject.transform.position = _gameObject.transform.position + new Vector3(gravityVector.x * Time.deltaTime, gravityVector.y * Time.deltaTime, gravityVector.z * Time.deltaTime);
	}
}

// rotate player depending on position direction vector
// first just keep going in this direction...
// explore rigidbody instead ofcharacter controller (see if playercontroller can be used without character controller as well)

// leftoff: rotation is applying correctly, but model is not rotating...