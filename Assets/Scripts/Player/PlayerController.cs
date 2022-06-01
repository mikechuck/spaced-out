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

	public float MovementSpeed = 1;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private float Gravity = -50f;
	private float jumpForce = 30f;
	private float groundHeight = 4f;
	private float velocity = 0;
	private Vector3 playerVelocity;
	private float xRotate = 0.0f;
	private float yRotate = 0.0f;
	private Ray ray;
	private float interactionDistance = 10f;

	private void Awake() {
		view = GetComponent<PhotonView>();
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
		bool isGrounded = Physics.Raycast(transform.position, -transform.up, out hitData, groundHeight);

		if (isGrounded) {
			if (playerVelocity.y < 0) {
				playerVelocity.y = 0f;
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				playerVelocity.y += Mathf.Sqrt(jumpForce * -Gravity);
			}
		}
        playerVelocity.y += Gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
		

		// Movement
		float currentMovementSpeed = MovementSpeed;
		if (Input.GetKey(KeyCode.LeftShift)) {
			currentMovementSpeed = MovementSpeed * 2;
		} else {
			currentMovementSpeed = MovementSpeed;
		}

		float horizontal = Input.GetAxis("Horizontal") * currentMovementSpeed;
		float vertical = Input.GetAxis("Vertical") * currentMovementSpeed;
		characterController.Move((transform.right * horizontal + transform.forward * vertical) * Time.deltaTime);

		// Camera movement
		xRotate += Input.GetAxis("Mouse X") * horizontalSpeed;
		yRotate -= Input.GetAxis("Mouse Y") * verticalSpeed;
		transform.eulerAngles = new Vector3 (0.0f, xRotate, 0.0f);
		yRotate = Mathf.Clamp (yRotate, -90, 90);
		cam.transform.eulerAngles = new Vector3 (yRotate, xRotate, 0.0f);

		// Move head aim target as well (vertical)
		HeadAnimationTarget.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, 5) );
	}
}