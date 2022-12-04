using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerController : PhysicsObject
{
	public GameObject HudPrefab;
	public Camera cam;
	public GameObject HeadAnimationTarget;
	public TextMeshProUGUI playerName;
	public float movementSpeed;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private CharacterController characterController;
	private InventoryManager inventoryManager;
	private GameObject HUD;
	private HUDManager hudManager;
	private GameManager gameManager;
	private Animator _animator;
	// private PhotonView view;
	private Ray ray;
	// private float Gravity = -50f;
	private float jumpForce = 20f;
	private float velocity = 0;
	private float xRotate = 0.0f;
	private float yRotate = 0.0f;
	private float interactionDistance = 10f;
	private bool canMovePlayer = true;
	private float horizontalInput;
	private float verticalInput;

	private void Awake() {
		_animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		inventoryManager = gameObject.GetComponent<InventoryManager>();
	}

	private void Start() {
		_gameObject = gameObject;
		characterController = GetComponent<CharacterController>();
		if (HudPrefab != null) {
			SetHudManager();
		} else {
			Debug.LogWarning("HudPrefab missing from PlayerController");
		}
	}
	
	void Update() {
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
		// Call Input code
		if (IsOwner) {
			CheckMovementInput();
			CheckItemInteraction();
			ApplyGravity();
		}
	}

	void CalledOnLevelWasLoaded() {
		SetHudManager();
	}

	private void SetPlayerCoords() {
		Hashtable coordsHash = new Hashtable();
		coordsHash.Add("coordX", transform.position.x);
		coordsHash.Add("coordY", transform.position.z);
		// need to convert to different hashtable type...
		// PhotonNetwork.LocalPlayer.SetCustomProperties(coordsHash);
	}

	private void SetHudManager() {
		HUD = Instantiate(this.HudPrefab);
		hudManager = HUD.GetComponent<HUDManager>();
		hudManager.SetPlayerController(this);
		hudManager.SetInventoryManager(inventoryManager);
		inventoryManager.SetHudManager(hudManager);
	}

	private void CheckItemInteraction() {
		GameObject hitObject = CastRay();
		if (hitObject) {
			HandleObjectHit(hitObject);
		} else {
			// hudManager.ShowItemInfo("");
		}
	}

	private GameObject CastRay() {
		ray = new Ray(cam.transform.position, transform.forward);
		Debug.DrawRay(cam.transform.position, transform.forward, Color.green);
		RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData)) {
			if (hitData.collider.gameObject.layer == 6) {
				if (hitData.distance <= interactionDistance) {
					return hitData.collider.gameObject;
				}
			}
        }
		return null;
	}

	private void HandleObjectHit(GameObject hitObject) {
		Transform parent = hitObject.gameObject.transform.parent;
		// hudManager.ShowItemInfo(parent.name);

		switch(hitObject.gameObject.tag) {
			case "Tree":
				// tree hit
				break;
			case "Item":
				// hudManager.ShowItemInfo(parent.name);
				break;
			default:
				break;
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
				_velocity.y += Mathf.Sqrt(jumpForce * -50f);
				// characterController.Move(_velocity * Time.deltaTime);
			}
		}
		if (isGrounded2) {
			if (_velocity.y < 0) {
				_velocity.y = 0f;
			}
			canMovePlayer = true;
		} else {
			canMovePlayer = false;
		}

        // characterController.Move(_velocity * Time.deltaTime);

		if (canMovePlayer) {
			horizontalInput = Input.GetAxis("Horizontal");
			verticalInput = Input.GetAxis("Vertical");

			// Sprinting
			if (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0) {
				verticalInput *= 2f;
				horizontalInput = 0f;
			}
			// Walking backwards
			if (verticalInput < 0) {
				verticalInput /= 1.5f;
			}
		}
		// characterController.Move((transform.right * horizontalInput * movementSpeed + transform.forward * verticalInput * movementSpeed) * Time.deltaTime);


		// Camera movement
		xRotate += Input.GetAxis("Mouse X") * horizontalSpeed;
		yRotate -= Input.GetAxis("Mouse Y") * verticalSpeed;
		transform.eulerAngles = new Vector3 (0.0f, xRotate, 0.0f);
		yRotate = Mathf.Clamp (yRotate, -80, 60);
		cam.transform.eulerAngles = new Vector3 (yRotate, xRotate, 0.0f);

		// Move head aim target as well (vertical)
		// HeadAnimationTarget.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, 50) );
	
		// Animating
		if (isGrounded1) {
			_animator.SetFloat("VelocityForward", verticalInput, 0.1f, Time.deltaTime);
			_animator.SetFloat("VelocitySide", horizontalInput, 0.1f, Time.deltaTime);
		}
	}
}