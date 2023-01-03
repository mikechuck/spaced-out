using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 9.8f;
	protected Rigidbody _rigidbody;
	protected Vector3 _worldUp;
	private bool _isGrounded = false;
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();
	public NetworkVariable<bool> IsGrounded = new NetworkVariable<bool>();

	public override void OnNetworkSpawn()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		// if (!IsOwner) return;
		// ApplyPhysics();
	}

	private void OnCollisionEnter(Collision collision)
	{
		_isGrounded = true;
		// IsGrounded.Value = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		_isGrounded = false;
		// IsGrounded.Value = false;
	}

	private void ApplyPhysics()
	{
		_worldUp = transform.position.normalized;
		if (!_isGrounded)
		{
			_rigidbody.AddForce(-_worldUp * _gravityMagnitude);
		}
		// ApplyGravityServerRpc();
		ApplyRotationServerRpc();
		// transform.position = Position.Value;
		transform.rotation = Rotation.Value;
	}

	// [ServerRpc]
	// private void ApplyGravityServerRpc(ServerRpcParams rpcParams = default)
	// {
	// 	if (!IsGrounded.Value)
	// 	{
	// 		_rigidbody.AddForce(-_worldUp * _gravityMagnitude * Time.deltaTime);
	// 		// Position.Value = transform.position;
	// 	}
	// }

	[ServerRpc]
	private void ApplyRotationServerRpc(ServerRpcParams rpcParams = default)
	{
		Rotation.Value = Quaternion.FromToRotation(Vector3.up, _worldUp);
	}
}