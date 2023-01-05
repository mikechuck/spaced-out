using Unity.Netcode;
using UnityEngine;

public class PlayerController: PhysicsObject
{

	#region Fields

	[SerializeField] private Camera _cam;
	[SerializeField] private GameObject _headAnimationTarget;
	[SerializeField] private GameObject _hudPrefab;
	private GameManager _gameManager;
	private HUDManager _hudManager;
	private GameObject _hud;
	private InventoryManager _inventoryManager;
	private CharacterController _characterController;
	private Animator _animator;
	private Vector3 _velocity;
	private float _movementSpeed = 5f;
	private float _jumpForce = 2000f;
	private float _horizontalInput;
	private float _verticalInput;
	private float _horizontalSpeed = 1f;
	private float _verticalSpeed = 1f;
	private float _xRotate = 0.0f;
	private float _yRotate = 0.0f;
	private float _itemInteractionDistance = 10f;
	private bool _canMovePlayer = true;
	private Vector3 _playerLastPosition = new Vector3(0f, 0f, 0f);

	#endregion

	#region Network Variables

	private NetworkVariable<Quaternion> _camRotation = new NetworkVariable<Quaternion>();
	private NetworkVariable<NetworkString> _playerName = new NetworkVariable<NetworkString>();

	#endregion

	#region Lifecyles

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		DisableOtherPlayerCams();
		SetInitialPosition();
	}

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_inventoryManager = gameObject.GetComponent<InventoryManager>();
		SetHudManager();
		SetHudPlayerName();
	}

	private void Update()
	{
		MovePlayer();
	}

	#endregion

	#region Methods

	private void SetHudManager()
	{
		if (_hudPrefab != null) {
			_hud = Instantiate(this._hudPrefab);
			_hudManager = _hud.GetComponent<HUDManager>();
			_hudManager.SetPlayerController(this);
			_inventoryManager.SetHudManager(_hudManager);
			_hudManager.SetInventoryManager(_inventoryManager);
		} else {
			Debug.LogWarning("HudPrefab missing from PlayerController");
		}	
	}

	private void SetHudPlayerName()
	{
		_hudManager.SetPlayerName(_playerName.Value);
	}

	private void DisableOtherPlayerCams()
	{
		if (!IsOwner)
		{
			GameObject cam = GameObject.Find("Player Camera");
			cam.SetActive(false);
		}
	}

	private void SetInitialPosition()
	{
		float randomZ = Random.Range(0f, 5f);
		float randomX = Random.Range(0f, 5f);
		Vector3 newPosition = new Vector3(randomX, 800f, randomZ);
		if (IsClient && IsOwner)
		{
			Debug.Log("sending initial position to server");
			Debug.Log(newPosition);
			SetInitialPositionServerRpc(newPosition);
		}
	}

	private void MovePlayer()
	{
		// Player and Camera Rotation
		_xRotate += Input.GetAxis("Mouse X") * _horizontalSpeed;
		_yRotate -= Input.GetAxis("Mouse Y") * _verticalSpeed;

		// Player movement
		_horizontalInput = Input.GetAxis("Horizontal");
		_verticalInput = Input.GetAxis("Vertical");
		ApplyPlayerMovement();
	}

	private void ApplyPlayerMovement()
	{
		Vector3 newPosition = transform.position + (transform.right * _horizontalInput + transform.forward * _verticalInput) * _movementSpeed * Time.deltaTime;
		// Only update position if it's changed
		if (newPosition != transform.position)
		{
			if (IsServer)
			{
				transform.position = _playerLastPosition;
			}
			else if (IsClient && IsOwner)
			{
				ApplyPlayerMovementServerRpc(newPosition);
			}
		}
	}

	#endregion

	#region RPC connections

	[ServerRpc]
	private void RotatePlayerServerRpc(float xRotate, float yRotate)
	{
		Rotation.Value = Quaternion.Euler(0f, xRotate, 0f);
		// _camRotation.Value = Quaternion.Euler(yRotate, xRotate, 0f);
	}

	[ServerRpc]
	private void SetInitialPositionServerRpc(Vector3 initialPosition)
	{
		transform.position = initialPosition;
		_playerLastPosition = initialPosition;
	}

	[ServerRpc]
	private void ApplyPlayerMovementServerRpc(Vector3 newPosition)
	{
		_playerLastPosition = newPosition;
	}

	[ServerRpc]
	private void PlayerJumpServerRpc()
	{
		Debug.Log("jumping server");
		_rigidbody.AddForce(_worldUp * _jumpForce);
	}

	#endregion
}

// leftoff: finish hooking up playername to hud (should be good, just test)

// leftoff: need to finish implementing initial position (to start planet spawning)
// - FIRST: switch away from network transform/rigidbody and go back to pure networkvariables (best performance)
