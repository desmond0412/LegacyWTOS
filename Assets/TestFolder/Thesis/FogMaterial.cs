using UnityEngine;
using System.Collections;

public class FogMaterial : MonoBehaviour {
	//public properties
	public float fogSpeedMultiplier = 1.0f;
	public float fogAnimationSpeed = 0.05f;
	public Texture[] spritesheet;
	public Material mat;
	public int textureRow = 1,textureCol = 1;

	Renderer rend;
	int index = 0,animSkip = 0;
	float xSpan,ySpan;
	float animationTime;
	float offsetAnimation;

	void Start () {
		rend = GetComponent<Renderer> ();
		if (rend) {
			rend.material = mat;
			xSpan = 1.0f / (float)textureCol;
			ySpan = 1.0f / (float)textureRow;
			rend.material.mainTextureScale = new Vector2 (xSpan, ySpan);

			rend.sharedMaterial = rend.material;
			mat = rend.sharedMaterial;
		}

		animationTime = 0.0f;
		offsetAnimation = 0.0f;
	}

	void FixedUpdate(){
		animationTime += Time.fixedDeltaTime;

		if (animationTime >= fogAnimationSpeed && fogAnimationSpeed > 0.0f) {
			animSkip +=	(int)(animationTime / fogAnimationSpeed);
			animationTime %= fogAnimationSpeed;
			if (spritesheet.Length > 0) {
				mat.mainTexture = spritesheet [animSkip % spritesheet.Length];
			}
			animSkip %= Mathf.Max(spritesheet.Length,(textureCol * textureRow));
		}

		offsetAnimation -= fogSpeedMultiplier * Time.fixedDeltaTime;
		mat.mainTextureOffset = new Vector2 ((animSkip%textureCol) * xSpan + offsetAnimation, ((textureRow - 1 - (animSkip/textureRow)) % textureRow) * ySpan);
	}
}
