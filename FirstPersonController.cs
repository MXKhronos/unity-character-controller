using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirstPersonController : NetworkBehaviour {
	//Public
	public float mouseSensitivity = 1.25f;

	//Private
	CharacterController characterController;
	GameObject characterBody;
	GameObject characterCamera;

	float camVertLimit = 89.9f;

    float defaultSpeed = 2.5f;
	float sprintSpeed = 1.5f;
	float jumpForce = 6f;
	float airTime = 0;
	
	bool cursorEnabled = false;

	float vMouseInput, hMouseInput, vMovementInput, hMovementInput, camVertRotation;
	Vector3 prevMovementVector, movementVector, gravitationalVector;

	void UpdateCursor() {
		Cursor.visible = cursorEnabled;
		Cursor.lockState = (cursorEnabled) ? (CursorLockMode.None) : (Screen.fullScreen) ? (CursorLockMode.Locked) : (CursorLockMode.Confined);
	}

	void Start () {
		if (!isLocalPlayer) { return; }
		characterController = gameObject.GetComponent<CharacterController>();
		characterBody = characterController.gameObject;
		characterCamera = gameObject.GetComponentInChildren<Camera>(true).gameObject;
		characterCamera.GetComponent<Camera>().enabled = true;
		characterCamera.GetComponent<AudioListener>().enabled = true;
		UpdateCursor();
	}
	
	bool IsOnGround() {
		return Physics.Raycast(transform.position, Vector3.down, 1.2f);
	}

	void UpdateControl() {
		hMouseInput = Input.GetAxisRaw("Mouse X");
		vMouseInput = Input.GetAxisRaw("Mouse Y");

		camVertRotation -= vMouseInput * mouseSensitivity;
		camVertRotation = Mathf.Clamp(camVertRotation, -camVertLimit, camVertLimit);
		characterCamera.transform.localRotation = Quaternion.Euler(camVertRotation, 0, 0);
		characterBody.transform.Rotate(0, hMouseInput * mouseSensitivity, 0);

		vMovementInput = Input.GetAxisRaw("Vertical");
		hMovementInput = Input.GetAxisRaw("Horizontal");

		float movementSpeed = defaultSpeed;
		bool isGrounded = IsOnGround();

		if (Input.GetKey(KeyCode.LeftShift)) {
			movementSpeed += sprintSpeed;
		}
		movementVector = Vector3.Lerp(prevMovementVector, characterBody.transform.rotation * new Vector3(hMovementInput, 0, vMovementInput).normalized * movementSpeed, 0.15f);
		prevMovementVector = movementVector;

		if (characterController.isGrounded) {
			gravitationalVector = new Vector3(0, -0.01f, 0);
			if (Input.GetAxis("Jump") > 0) {
				gravitationalVector += new Vector3(0, jumpForce, 0);
			}
		} else {
			gravitationalVector += Physics.gravity * Time.fixedDeltaTime;
		}
		movementVector += gravitationalVector;
		characterController.Move(movementVector * Time.fixedDeltaTime);

		if (transform.position.y < -10) {
			transform.position = new Vector3(0, 1, 0);
		}
	}

	void Update() {
		if (!isLocalPlayer) { return; }
		UpdateControl();
		UpdateCursor();
		if (Input.GetKeyDown(KeyCode.Escape)) {
			cursorEnabled = !cursorEnabled;
		}
	}
}
