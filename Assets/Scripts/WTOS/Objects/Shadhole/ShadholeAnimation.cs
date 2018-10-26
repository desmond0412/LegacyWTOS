using UnityEngine;
using System.Collections;
using System.Linq;



public class ShadholeAnimation : MonoBehaviour {

	public Texture[] spritesheet;
	public int fps = 60;
	public float speedMultiplier = 1f;

	Material mat;
	int index = 0;
	float timeUntilNextFrame = 0f;

	void Awake (){
		mat = GetComponent<Renderer>().material;
		if (spritesheet.Length > 0) {
			spritesheet = spritesheet.OrderBy (x => x.name).ToArray ();
		}
		timeUntilNextFrame = 1f / fps;
	}


	void Update(){
		if (mat != null && spritesheet.Length > 0) {
			while (timeUntilNextFrame < 0f) {
				index = (index < spritesheet.Length-1) ? index + 1 : 0;

				mat.mainTexture = spritesheet [index];
				mat.mainTextureOffset -= new Vector2 (0, 1f/fps * speedMultiplier);
				timeUntilNextFrame += 1f/fps;
			}
			timeUntilNextFrame -= Time.deltaTime;
		}
	}


}