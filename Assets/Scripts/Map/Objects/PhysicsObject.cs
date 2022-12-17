using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 5f;
	private Vector3 _worldUp;
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

	void Update()
	{
		ApplyPhysics();
	}

	protected void ApplyPhysics()
	{
		_worldUp = transform.position.normalized;
		if (IsOwner)
		{
			ApplyGravityServerRpc();
			// ApplyRotationServerRpc();
		}
		transform.position = Position.Value;
		transform.rotation = Rotation.Value;
	}

	[ServerRpc]
	private void ApplyGravityServerRpc(ServerRpcParams rpcParams = default)
	{
		Position.Value += -_worldUp * _gravityMagnitude * Time.deltaTime;
	}

	[ServerRpc]
	private void ApplyRotationServerRpc(ServerRpcParams rpcParams = default)
	{
		// Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, _worldUp);
		// Rotation.Value = newRotation;
	}
}