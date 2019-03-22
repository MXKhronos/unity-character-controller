using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService : MonoBehaviour {
	NetworkManager networkManager;
	Camera networkCamera;

	void Start () {
		networkManager = gameObject.GetComponent<NetworkManager>();
		networkCamera = gameObject.GetComponent<Camera>();
	}

	void Update() {
		if (networkManager.client != null && networkManager.client.isConnected) {
			networkCamera.enabled = false;
		} else {
			networkCamera.enabled = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
