using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;
	private GameObject HUD;
	private HUDManager hudManager;
	private GameManager gameManager;
	public Camera cam;
	public GameObject HeadAnimationTarget;
	PhotonView view;
	public TextMeshProUGUI playerName;
	Animator _animator;

	public float movementSpeed;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private float Gravity = -50f;
	private float jumpForce = 20f;
	private float groundHeight = 3.3f;
	private float velocity = 0;
	private Vector3 playerVelocity;
	private float xRotate = 0.0f;
	private float yRotate = 0.0f;
	private Ray ray;
	private float interactionDistance = 10f;
	private float airMovementSpeed = 0f;
	private bool canMovePlayer = true;

	// Global movement directions
	private float horizontal;
	private float vertical;
	private bool isJumping;

	// values for character rotation smoothing
	private float interp = 0;
	private float rotationSpeed = 0.5f;

	private void Awake() {
		view = GetComponent<PhotonView>();
		_animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Start() {
		characterController = GetComponent<CharacterController>();
		HUD = GameObject.Find("HUD");
		hudManager = HUD.GetComponent<HUDManager>();
	}
	
	void Update() {
		if (!view.IsMine) {
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
		if (view.IsMine) {
			CheckMovementInput();
			CheckItemInteraction();
		}
	}

	private void CheckItemInteraction() {
		GameObject hitObject = CastRay();
		if (hitObject) {
			HandleObjectHit(hitObject);
		} else {
			hudManager.ShowItemInfo("");
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
		hudManager.ShowItemInfo(parent.name);

		switch(hitObject.gameObject.tag) {
			case "Tree":
				TreeState treeState = parent.gameObject.GetComponent<TreeState>();

				if (Input.GetKeyDown(KeyCode.Mouse0)) {
					treeState.DecreaseHP(5);
				}
				break;
			case "Item":
				hudManager.ShowItemInfo(parent.name);
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
				playerVelocity.y += Mathf.Sqrt(jumpForce * -Gravity);
				characterController.Move(playerVelocity * Time.deltaTime);
			}
		}
		if (isGrounded2) {
			if (playerVelocity.y < 0) {
				playerVelocity.y = 0f;
			}
			canMovePlayer = true;
		} else {
			canMovePlayer = false;
		}
		
        playerVelocity.y += Gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

		if (canMovePlayer) {
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			// Sprinting
			if (Input.GetKey(KeyCode.LeftShift) && vertical > 0) {
				vertical *= 2f;
				horizontal = 0f;
			}
			// Walking backwards
			if (vertical < 0) {
				vertical /= 1.5f;
			}
		}
		characterController.Move((transform.right * horizontal * movementSpeed + transform.forward * vertical * movementSpeed) * Time.deltaTime);


		// Camera movement
		xRotate += Input.GetAxis("Mouse X") * horizontalSpeed;
		yRotate -= Input.GetAxis("Mouse Y") * verticalSpeed;
		transform.eulerAngles = new Vector3 (0.0f, xRotate, 0.0f);
		yRotate = Mathf.Clamp (yRotate, -80, 60);
		cam.transform.eulerAngles = new Vector3 (yRotate, xRotate, 0.0f);

		// Move head aim target as well (vertical)
		HeadAnimationTarget.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, 50) );
	
		// Animating
		if (isGrounded1) {
			_animator.SetFloat("VelocityForward", vertical, 0.1f, Time.deltaTime);
			_animator.SetFloat("VelocitySide", horizontal, 0.1f, Time.deltaTime);
		}

		transform.Rotate(0, 180, 0, Space.Self);
	}
}