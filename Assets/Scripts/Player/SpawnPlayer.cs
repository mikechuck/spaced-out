using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnPlayer : NetworkBehaviour
{
	[SerializeField] private NetworkObject playerPrefab;

	public void Spawn() {
		if (!IsOwner) { return; }

		// ray cast from origin in random 360 direction (all directions)
		// find coord where it hits, get the normal of the vector and use as spawn rotation in instantiate
		// in playercontroller:
		// - have code in update that always set normal of current position to be up
		// - for gravity update, use x, y, z components of normal to subtract from player coords (multiplied by gravity as well)

		SpawnPlayerInstanceServerRpc();
	}

	[ServerRpc]
	void SpawnPlayerInstanceServerRpc(ServerRpcParams rpcParams = default)
	{
		// SpawnPlayerInstance();
		NetworkObject playerInstance = Instantiate(playerPrefab);
		playerInstance.SpawnWithOwnership(OwnerClientId);
	}
}