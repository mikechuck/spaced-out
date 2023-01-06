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
	[SerializeField] private float _movementSpeed = 5f;
	private float _jumpForce = 2000f;
	private float _horizontalSpeed = 1f;
	private float _verticalSpeed = 1f;
	private float _xRotate = 0.0f;
	private float _yRotate = 0.0f;
	private float _itemInteractionDistance = 10f;
	private bool _canMovePlayer = true;

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
		if (IsServer)
		{
			UpdateServer();
		}

		if (IsClient && IsOwner)
		{
			UpdateClient();
		}
	}

	#endregion

	#region Methods

	private void UpdateServer()
	{
		// transform.position = Position.Value;
		// _cam.transform.rotation = _camRotation.Value;
	}

	private void UpdateClient()
	{
		// Player and Camera Rotation
		_xRotate += Input.GetAxis("Mouse X") * _horizontalSpeed;
		_yRotate -= Input.GetAxis("Mouse Y") * _verticalSpeed;

		// Player movement
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 movementVector = (transform.right * horizontalInput + transform.forward * verticalInput) * _movementSpeed * Time.deltaTime;
		if (movementVector != Vector3.zero)
		{
			Vector3 newPosition = transform.position + movementVector;
			ApplyPlayerMovementServerRpc(newPosition);
		}
		if (_xRotate != 0f && _yRotate != 0f)
		{
			// _cam.transform.rotation = _camRotation.Value;
			RotatePlayerServerRpc(_xRotate, _yRotate);
		}
	}

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
		float randomZ = Random.Range(0f, 500f);
		float randomX = Random.Range(0f, 500f);
		Vector3 newPosition = new Vector3(randomX, 700f, randomZ);
		if (IsClient && IsOwner)
		{
			SetInitialPositionServerRpc(newPosition);
		}
	}

	#endregion

	#region RPC connections

	[ServerRpc]
	private void SetInitialPositionServerRpc(Vector3 initialPosition)
	{
		transform.position = initialPosition;
		Position.Value = initialPosition;
	}

	[ServerRpc]
	private void RotatePlayerServerRpc(float xRotate, float yRotate)
	{
		Rotation.Value = Quaternion.Euler(0f, xRotate, 0f);;
		_camRotation.Value = Quaternion.Euler(yRotate, yRotate, 0f);
	}

	[ServerRpc]
	private void ApplyPlayerMovementServerRpc(Vector3 newPosition)
	{
		Position.Value = newPosition;
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

// leftoff: fix gravity

// leftoff: complete cam rotation
