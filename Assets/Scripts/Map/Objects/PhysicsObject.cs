using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PhysicsObject : NetworkBehaviour
{
	private float _gravityMagnitude = 9.8f;
	protected Rigidbody _rigidbody;
	public NetworkVariable<Vector3> WorldUp = new NetworkVariable<Vector3>();
	public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
	public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();
	protected NetworkVariable<bool> IsGrounded = new NetworkVariable<bool>();

	#region Lifecycle

	public override void OnNetworkSpawn()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	protected void Update()
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
		// Quaternion.FromToRotation(Vector3.up, WorldUp.Value);
		// Debug.Log("WorldUp.Value: " + WorldUp.Value);
		// Debug.Log("transform.pos: " + transform.position.normalized);
		WorldUp.Value = transform.position.normalized;

		if (!IsGrounded.Value)
		{
			// TODO: need to find way to sync this while still using Position network variables
			// _rigidbody.AddForce(gravityForce);
		}

		transform.position = Position.Value;
		transform.rotation = Rotation.Value;
	}

	private void UpdateClient()
	{
		Quaternion rotationDifference = Quaternion.FromToRotation(Vector3.up, WorldUp.Value);
		// Debug.Log("transform.up: " + transform.up);
		// Debug.Log("Rotation.Value: " + Rotation.Value.eulerAngles);
		Debug.DrawRay(transform.position, WorldUp.Value * 20, Color.green);
		Debug.DrawRay(transform.position, Rotation.Value.eulerAngles, Color.red);
		// Quaternion newRotation = Rotation
		// Debug.DrawRay(transform.position, newPosition2.eulerAngles, Color.yellow);
		// Debug.DrawRay(transform.position, newPosition3.eulerAngles, Color.white);
		

		// Quaternion dotPosition = Quaternion.Dot(Rotation.Value, rotationChange);

		// if (!IsGrounded.Value)
		// {
		// 	Vector3 gravityMovementVector = - worldUp * _gravityMagnitude * 0.05f;
		// 	ApplyGravityServerRpc(gravityMovementVector);
		// }
		// ApplyRotationServerRpc(worldUp);
	}

	#endregion

	#region Server RPC methods

	[ServerRpc]
	private void ApplyGravityServerRpc(Vector3 gravityForce)
	{
		
	}

	// [ServerRpc]
	// private void ApplyRotationServerRpc(Vector3 worldUp)
	// {
		// Rotation.Value = Quaternion.FromToRotation(Vector3.up, worldUp);

		// Rotation.Value = Quaternion.LookRotation(transform.forward, worldUp);
		// Rotation.Value = Quaternion.Lerp(Rotation.Value, rotationChange * Rotation.Value, Time.deltaTime);
		// Rotation.Value = Quaternion.Dot(Rotation.Value, rotationChange);
		// Debug.Log("rotation.value: " + Rotation.Value);
		// transform.up = worldUp;
		// Quaternion desiredRotation = Quaternion.LookRotation(Vector3.up, worldUp);
		// Rotation.Value = Quaternion.Euler(0f, 0f, desiredRotation.eulerAngles.z + 90);
		// Rotation.Value = Quaternion.Euler(Rotation.Value.eulerAngles.x, Rotation.Value.eulerAngles.y, Rotation.Value.eulerAngles.z);
		// Debug.Log("transform.up: " + transform.up);
		// Debug.Log("Rotation.Value: " + Rotation.Value.eulerAngles);
		// WorldUp.Value = worldUp;
	// }

	#endregion
}