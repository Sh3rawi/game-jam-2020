using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
	public float speed = 1f;
	Material stars,grid;
	void Start() {
		var renderer = GetComponent<MeshRenderer>();
		grid = renderer.materials[1];
		stars = renderer.materials[2];
	}

	void Update() {
		SetOffset(grid);
		SetOffset(stars, 0f);
	}

	void SetOffset(Material mat, float speedModifier = 1f) {
		var offset = mat.mainTextureOffset;
		offset.y += Time.deltaTime / 10f * speed * speedModifier;
		mat.mainTextureOffset = offset;
	}
}
