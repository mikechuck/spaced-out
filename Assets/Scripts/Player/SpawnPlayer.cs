using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
	// [SerializeField] private NetworkObject playerPrefab;

	public void Spawn() {
		// if (!IsOwner) { return; }

		// ray cast from origin in random 360 direction (all directions)
		// find coord where it hits, get the normal of the vector and use as spawn rotation in instantiate

		// SpawnPlayerInstanceServerRpc();
	}

	// [ServerRpc]
	// void SpawnPlayerInstanceServerRpc(ServerRpcParams rpcParams = default)
	// {
	// 	Debug.Log("spawning...");
	// 	Debug.Log(OwnerClientId);
	// 	// GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
	// 	// SpawnPlayerInstance();
	// 	// NetworkObject playerInstance = Instantiate(playerPrefab);
	// 	// playerInstance.SpawnWithOwnership(OwnerClientId);
	// }
}