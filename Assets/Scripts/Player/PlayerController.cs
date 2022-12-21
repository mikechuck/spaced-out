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
	private float _jumpForce = 20f;
	private float _horizontalInput;
	private float _verticalInput;
	private float _horizontalSpeed = 1f;
	private float _verticalSpeed = 1f;
	private float _xRotate = 0.0f;
	private float _yRotate = 0.0f;
	private float _itemInteractionDistance = 10f;
	private bool _canMovePlayer = true;
	private bool _isGrounded1;
	private bool _isGrounded2;

	#endregion

	#region Network Variables

	public NetworkVariable<Quaternion> CamRotation = new NetworkVariable<Quaternion>();

	#endregion

	#region Lifecyles

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_inventoryManager = gameObject.GetComponent<InventoryManager>();
		SetHudManager();
	}

	protected override void Update()
	{
		base.Update();
		// if (!IsOwner) return;
		// CheckMovementInput();
		// CheckItemInteraction();

		// TODO: fix method to disable other player cams
		// DisablePlayerCams();
	}

	public void OnCollisionEnter(Collision collision)
	{
		// Debug.Log("collision");
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

	private void CheckMovementInput()
	{
		// Player and Camera Rotation
		_xRotate += Input.GetAxis("Mouse X") * _horizontalSpeed;
		_yRotate -= Input.GetAxis("Mouse Y") * _verticalSpeed;

		// Player movement
		_horizontalInput = Input.GetAxis("Horizontal");
		_verticalInput = Input.GetAxis("Vertical");

		RotatePlayer();
		HandleJumpInput();
		HandleMovementInput();
		AnimatePlayer();

		Vector3 movementVector = transform.position + (transform.right * _horizontalInput + transform.forward * _verticalInput) * _movementSpeed * Time.deltaTime;
		Vector3 newVelocity = _velocity + movementVector;
		// MovePlayerServerRpc(movementVector);
		// Debug.Log("transform.position (playercontroller): " + transform.position);
		transform.position = Position.Value;
	}

	private void HandleJumpInput()
	{
		// TODO: fix this to only have one bool check (after player is on top of planet)
		// Gravity + Jump force
		RaycastHit hitData;
		// Separate checks: one for velocity floor, one for letting rapid jumps
		_isGrounded1 = Physics.Raycast(transform.position, -transform.up, out hitData, 3.5f);
		_isGrounded2 = Physics.Raycast(transform.position, -transform.up, out hitData, 3.3f);
		if (_isGrounded1)
		{
			float velocityForward = _animator.GetFloat("VelocityForward");
			bool allowJump = velocityForward >= 0f;
			if (Input.GetKeyDown(KeyCode.Space) && allowJump) {
				_animator.SetTrigger("Jumping");
				_velocity.y += Mathf.Sqrt(_jumpForce * -50f);
			}
		}
		if (_isGrounded2)
		{
			if (_velocity.y < 0) {
				_velocity.y = 0f;
			}
			_canMovePlayer = true;
		} else {
			_canMovePlayer = false;
		}
	}

	private void HandleMovementInput()
	{
		if (_canMovePlayer)
		{
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
	}

	private void RotatePlayer()
	{
		if (IsOwner)
		{
			RotatePlayerServerRpc(_xRotate, _yRotate);
		}
		transform.rotation = Rotation.Value;
		_cam.transform.rotation = CamRotation.Value;
		// TODO: hook up head animation once rigging is done
		// _headAnimationTarget.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, 50) );
	}

	private void AnimatePlayer()
	{
		if (_isGrounded1)
		{
			_animator.SetFloat("VelocityForward", _verticalInput, 0.1f, Time.deltaTime);
			_animator.SetFloat("VelocitySide", _horizontalInput, 0.1f, Time.deltaTime);
		}
	}

	private void CheckItemInteraction()
	{
		GameObject hitObject = CastRay();
		if (hitObject)
		{
			HandleObjectHit(hitObject);
		} else {
			// _hudManager.ShowItemInfo("");
		}
	}

	private GameObject CastRay()
	{
		Ray ray = new Ray(_cam.transform.position, transform.forward);
		Debug.DrawRay(_cam.transform.position, transform.forward, Color.green);
		RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
		{
			if (hitData.collider.gameObject.layer == 6) {
				if (hitData.distance <= _itemInteractionDistance) {
					return hitData.collider.gameObject;
				}
			}
        }
		return null;
	}

	private void HandleObjectHit(GameObject hitObject)
	{
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

	#endregion

	#region RPC connections

	[ServerRpc]
	private void MovePlayerServerRpc(Vector3 playerPosition)
	{
		Position.Value = playerPosition;
	}

	[ServerRpc]
	private void RotatePlayerServerRpc(float xRotate, float yRotate)
	{
		Rotation.Value = Quaternion.Euler(0f, xRotate, 0f);
		CamRotation.Value = Quaternion.Euler(yRotate, xRotate, 0f);
	}

	#endregion
}

// leftoff: need to finish implementing initial position (to start planet spawning)
// try Start again, onnetworkspawn didn't work... look for other lifecycle methods?
// after that, fix movement inputs
