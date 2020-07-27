using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
	public float speed = 1f;
	Material mat;
	void Start() {
		var renderer = GetComponent<MeshRenderer>();
		mat = renderer.materials[0];
	}

	void Update() {
		SetOffset(mat);
	}

	void SetOffset(Material mat) {
		var offset = mat.mainTextureOffset;
		offset.y += Time.deltaTime / 10f * speed;
		mat.mainTextureOffset = offset;
	}
}
