using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;
	public float MovementSpeed = 1;
	public float jumpForce = 20f;
	public float Gravity = 9.8f;
	private float velocity = 0;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private float xRotation = 0.0f;
	public Camera cam;
	PhotonView view;
	public TextMeshProUGUI playerName;
	private Ray ray;
	private float interactionDistance = 5f;

	private void Awake() {
		view = GetComponent<PhotonView>();
	}

	private void Start() {
		characterController = GetComponent<CharacterController>();
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
		}

		CheckItemInteraction();
	}

	private void CheckItemInteraction() {
		if (Input.GetKey(KeyCode.Mouse0)) {
			GameObject hitItem = CastRay();
			if (hitItem) {
				Debug.Log("item hit!");
				Debug.Log(hitItem.gameObject.name);
			}
		}
	}

	private GameObject CastRay() {
		Debug.Log("casting ray");
		ray = new Ray(transform.position, transform.forward);
		RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData)){
			Debug.Log("hit");
			Debug.Log(hitData.distance);
			Debug.Log(interactionDistance);
			if (hitData.distance <= interactionDistance) {
				return hitData.collider.gameObject;
			}
        }
		return null;
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
		float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
		float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;
		transform.Rotate(0, mouseX, 0);
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(-mouseY, -90, 90);
		cam.transform.Rotate(xRotation, 0.0f, 0.0f);
	}
}