using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;
	public float MovementSpeed = 1;
	public float Gravity = 9.8f;
	private float velocity = 0;
	public float horizontalSpeed = 1f;
	public float verticalSpeed = 1f;
	private float xRotation = 0.0f;
	public Camera cam;
	PhotonView view;

	private void Awake() {
		
	}

	private void Start() {
		characterController = GetComponent<CharacterController>();
	}

	void Update() {
		view = GetComponent<PhotonView>();
		if (!view.IsMine && GetComponent<PlayerController>() != null)
		{
			Debug.Log("Destroying player scripts");
			PlayerController playerController = GetComponent<PlayerController>();
			Debug.Log(playerController);
			Destroy(playerController);

			GameObject cam = gameObject.transform.GetChild(0).GetChild(0).gameObject;
			Debug.Log("cam");
			Debug.Log(cam);
			Destroy(cam);
		}

		if (view.IsMine) {
			CheckInput();
		}
	}

	private void CheckInput() {
		float currentMovementSpeed = MovementSpeed;
		if (Input.GetKey(KeyCode.LeftShift)) {
			currentMovementSpeed = MovementSpeed * 2;
		} else {
			currentMovementSpeed = MovementSpeed;
		}

		float horizontal = Input.GetAxis("Horizontal") * currentMovementSpeed;
		float vertical = Input.GetAxis("Vertical") * currentMovementSpeed;
		characterController.Move((transform.right * horizontal + transform.forward * vertical) * Time.deltaTime);

		if (characterController.isGrounded) {
			velocity = 0;
		} else {
			velocity -= Gravity * Time.deltaTime;
			characterController.Move(new Vector3(0, velocity, 0));
		}

		float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
		float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

		// horizontal rotation when mouse moves
		transform.Rotate(0, mouseX, 0);

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(-mouseY, -90, 90);
		cam.transform.Rotate(xRotation, 0.0f, 0.0f);
	}
}
