using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 9.8f;
	private Vector3 _worldUp;
	private Rigidbody _rigidbody;
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

	public override void OnNetworkSpawn()
	{
		CheckAndSetComponents();
		SetInitialPositionServerRpc();
	}

	private void CheckAndSetComponents()
	{
		NetworkTransform networkTransform = GetComponent<NetworkTransform>();
		NetworkObject networkObject = GetComponent<NetworkObject>();
		NetworkRigidbody networkRigidbody = GetComponent<NetworkRigidbody>();
		Rigidbody rigidbody = GetComponent<Rigidbody>();

		if (networkTransform == null || networkObject == null || networkRigidbody == null || rigidbody == null)
		{
			Debug.LogError("Physics object missing networking components");
		}
		else
		{
			_rigidbody = rigidbody;
		}
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
		transform.rotation = Rotation.Value;
	}

	[ServerRpc]
	private void SetInitialPositionServerRpc()
	{
		transform.position = new Vector3(0f, 1000f, 0f);
	}

	[ServerRpc]
	private void ApplyGravityServerRpc(ServerRpcParams rpcParams = default)
	{
		_rigidbody.AddForce(-_worldUp * _gravityMagnitude);
	}

	[ServerRpc]
	private void ApplyRotationServerRpc(ServerRpcParams rpcParams = default)
	{
		Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, _worldUp);
		Rotation.Value = newRotation;
	}
}