using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterNetworkScript : NetworkBehaviour {
	[ClientRpc]
	void RpcSetColor(Color color) {
		foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>(true)) {
			renderer.material.color = color;
		}
	}

	[Command]
	void CmdSetColor(Color color) {
		if (color == null) {
			color = new Color(Random.value, Random.value, Random.value);
		}
		RpcSetColor(color);
	}

	void Start() {
		CmdSetColor(new Color(Random.value, Random.value, Random.value));
		if (!isLocalPlayer) {
			foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>(true)) {
				renderer.gameObject.layer = 0;
			}
		}
	}

	void Update() {
		if (isLocalPlayer && Input.GetKeyDown(KeyCode.R)) {
			CmdSetColor(new Color(Random.value, Random.value, Random.value));
		}
	}
}
