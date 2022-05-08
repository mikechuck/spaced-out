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
			CheckInput();
		}
	}

	private void CheckInput() {
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