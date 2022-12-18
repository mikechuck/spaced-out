using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 500f;
	private Vector3 _worldUp;
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

	public override void OnNetworkSpawn()
	{
		SetInitialPositionServerRpc();
	}

	protected virtual void Update()
	{
		ApplyPhysics();
	}

	private void ApplyPhysics()
	{
		_worldUp = transform.position.normalized;
		if (IsOwner)
		{
			ApplyGravityServerRpc();
			ApplyRotationServerRpc();
		}
		transform.position = Position.Value;
		transform.rotation = Rotation.Value;
	}

	[ServerRpc]
	private void SetInitialPositionServerRpc()
	{
		Position.Value = new Vector3(1000f, 1000f, 1000f);
	}

	[ServerRpc]
	private void ApplyGravityServerRpc(ServerRpcParams rpcParams = default)
	{
		Vector3 newPosition = -_worldUp * _gravityMagnitude * Time.deltaTime;
		Position.Value += newPosition;
	}

	[ServerRpc]
	private void ApplyRotationServerRpc(ServerRpcParams rpcParams = default)
	{
		Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, _worldUp);
		Rotation.Value = newRotation;
	}
}