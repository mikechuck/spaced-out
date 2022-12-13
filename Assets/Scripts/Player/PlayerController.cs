using Unity.Netcode;
using UnityEngine;

public class PlayerController: PhysicsObject
{
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
	private float _jumpForce = 20f;
	private float _horizontalInput;
	private float _verticalInput;
	private float _horizontalSpeed = 1f;
	private float _verticalSpeed = 1f;
	private float _xRotate = 0.0f;
	private float _yRotate = 0.0f;
	private float _itemInteractionDistance = 10f;
	private bool _canMovePlayer = true;

	private void Start() {
		// if (!IsOwner) return;
		_characterController = GetComponent<CharacterController>();
		_animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_inventoryManager = gameObject.GetComponent<InventoryManager>();
		SetHudManager();
	}

	public override void OnNetworkSpawn()
	{
		if (IsClient)
		{
			Rotation.OnValueChanged += UpdatePlayerRotation;
		}
	}

	private void Update()
	{
		// if (!IsOwner) return;
		// DisablePlayerCams();
		// SetInitialPosition();
		CheckMovementInput();
		// CheckItemInteraction();
	}

	// public override void OnNetworkSpawn()
	// {
	// 	SetInitialPosition();
	// }

	// public void SetInitialPosition()
	// {
	// 	if (IsOwner)
	// 	{
	// 		SetInitialPositionServerRpc();
	// 	}
	// 	transform.position = Position.Value;
	// }

	private void SetHudManager() {
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

	private void DisablePlayerCams()
	{
		if (!IsOwner) {
			if (GetComponent<PlayerController>() != null) {
				// Destroy other players' scripts
				PlayerController playerController = GetComponent<PlayerController>();
				Destroy(playerController);
				// Disable other players' camera
				GameObject cam = gameObject.transform.GetChild(0).gameObject;
				cam.SetActive(false);
			}
		}
	}

	private void CheckMovementInput() {
		// Gravity + Jump force
		RaycastHit hitData;
		// Separate checks: one for velocity floor, one for letting rapid jumps
		bool isGrounded1 = Physics.Raycast(transform.position, -transform.up, out hitData, 3.5f);
		bool isGrounded2 = Physics.Raycast(transform.position, -transform.up, out hitData, 3.3f);
		if (isGrounded1) {
			float velocityForward = _animator.GetFloat("VelocityForward");
			bool allowJump = velocityForward >= 0f;
			if (Input.GetKeyDown(KeyCode.Space) && allowJump) {
				_animator.SetTrigger("Jumping");
				_velocity.y += Mathf.Sqrt(_jumpForce * -50f);
				// _characterController.Move(_velocity * Time.deltaTime);
			}
		}
		if (isGrounded2) {
			if (_velocity.y < 0) {
				_velocity.y = 0f;
			}
			_canMovePlayer = true;
		} else {
			_canMovePlayer = false;
		}

        _characterController.Move(_velocity * Time.deltaTime);

		if (_canMovePlayer) {
			_horizontalInput = Input.GetAxis("Horizontal");
			_verticalInput = Input.GetAxis("Vertical");

			// Sprinting
			if (Input.GetKey(KeyCode.LeftShift) && _verticalInput > 0) {
				_verticalInput *= 2f;
				_horizontalInput = 0f;
			}
			// Walking backwards
			if (_verticalInput < 0) {
				_verticalInput /= 1.5f;
			}
		}
		_characterController.Move((transform.right * _horizontalInput * _movementSpeed + transform.forward * _verticalInput * _movementSpeed) * Time.deltaTime);


		// Camera movement
		_xRotate += Input.GetAxis("Mouse X") * _horizontalSpeed;
		_yRotate -= Input.GetAxis("Mouse Y") * _verticalSpeed;
		
		RotatePlayer();
		RotateCamera();
		// Move head aim target as well (vertical)
		// _headAnimationTarget.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, 50) );
	
		// Animating
		if (isGrounded1) {
			_animator.SetFloat("VelocityForward", _verticalInput, 0.1f, Time.deltaTime);
			_animator.SetFloat("VelocitySide", _horizontalInput, 0.1f, Time.deltaTime);
		}
	}

	private void RotatePlayer()
	{
		if (IsOwner) {
			Debug.Log("sending update message: " + _xRotate);
			RotatePlayerServerRpc(_xRotate);
		}
	}

	private void UpdatePlayerRotation(Quaternion previous, Quaternion current)
	{
		transform.rotation = Quaternion.Slerp(previous, current, Time.time * 0.01f);;
	}

	private void RotateCamera()
	{
		_yRotate = Mathf.Clamp (_yRotate, -80, 60);
		_cam.transform.eulerAngles = new Vector3 (_yRotate, _xRotate, 0.0f);
	}

	private void CheckItemInteraction() {
		GameObject hitObject = CastRay();
		if (hitObject) {
			HandleObjectHit(hitObject);
		} else {
			// _hudManager.ShowItemInfo("");
		}
	}

	private GameObject CastRay() {
		Ray ray = new Ray(_cam.transform.position, transform.forward);
		Debug.DrawRay(_cam.transform.position, transform.forward, Color.green);
		RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData)) {
			if (hitData.collider.gameObject.layer == 6) {
				if (hitData.distance <= _itemInteractionDistance) {
					return hitData.collider.gameObject;
				}
			}
        }
		return null;
	}

	private void HandleObjectHit(GameObject hitObject) {
		Transform parent = hitObject.gameObject.transform.parent;
		// _hudManager.ShowItemInfo(parent.name);

		switch(hitObject.gameObject.tag) {
			case "Tree":
				// tree hit
				break;
			case "Item":
				// _hudManager.ShowItemInfo(parent.name);
				break;
			default:
				break;
		}
	}

	#region RPC connections
	[ServerRpc]
	private void SetInitialPositionServerRpc()
	{
		Position.Value = new Vector3(100f, 100f, 100f);
	}

	[ServerRpc]
	private void RotatePlayerServerRpc(float xRotate)
	{
		// Debug.Log("xRotate:" + xRotate);
		// Rotation.Value = new Vector3 (0.0f, _xRotate, 0.0f);
		Rotation.Value = Quaternion.Euler(0f, xRotate, 0f);
		Debug.Log("changing rotation:");
		Debug.Log(Rotation.Value);
		// Debug.Log(Rotation.Value);
	}
	#endregion
}

// leftoff: need to finish implementing initial position (to start planet spawning)
// try Start again, onnetworkspawn didn't work... look for other lifecycle methods?
// after that, fix movement inputs

// leftoff: lag is coming from second rotation from camera, need to fix this or combine into one single update. then continue organizing player script
