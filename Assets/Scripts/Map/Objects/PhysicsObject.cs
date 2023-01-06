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
	// private bool _isGrounded = false;
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();
	protected NetworkVariable<bool> IsGrounded = new NetworkVariable<bool>();

	#region Lifecycle

	public override void OnNetworkSpawn()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (IsServer)
		{
			UpdateServer();
		}
		if (IsOwner && IsClient)
		{
			UpdateClient();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		IsGrounded.Value = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		IsGrounded.Value = false;
	}

	#endregion

	#region Methods

	private void UpdateServer()
	{
		transform.position = Position.Value;
		transform.rotation = Rotation.Value;
	}

	private void UpdateClient()
	{
		_worldUp = transform.position.normalized;
		if (!IsGrounded.Value)
		{
			Vector3 gravityMovementVector = -_worldUp * _gravityMagnitude * 0.05f;
			ApplyGravityServerRpc(gravityMovementVector);
		}
		ApplyRotationServerRpc(_worldUp);
	}

	#endregion

	#region Server RPC methods

	[ServerRpc]
	private void ApplyGravityServerRpc(Vector3 gravityForce)
	{
		// TODO: need to find way to sync this while still using Position network variables
		// _rigidbody.AddForce(gravityForce);
	}

	[ServerRpc]
	private void ApplyRotationServerRpc(Vector3 worldUp)
	{
		// Rotation.Value = Quaternion.FromToRotation(Vector3.up, worldUp);
	}

	#endregion
}