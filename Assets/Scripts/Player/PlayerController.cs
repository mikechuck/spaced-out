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
	public float MovementSpeed = 1;
	public float jumpForce = 20f;
	public float Gravity = 9.8f;
	private float velocity = 0;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private float xRotate = 0.0f;
	private float yRotate = 0.0f;
	public Camera cam;
	PhotonView view;
	public TextMeshProUGUI playerName;
	private Ray ray;
	private float interactionDistance = 10f;

	private void Awake() {
		view = GetComponent<PhotonView>();
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
		if (characterController.isGrounded) {
			velocity = 0;
		} else {
			if (Input.GetKeyDown(KeyCode.Space)) {
				velocity = jumpForce;
			}
			velocity -= Gravity;
			characterController.Move(new Vector3(0, velocity, 0));
		}
		

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
	}
}