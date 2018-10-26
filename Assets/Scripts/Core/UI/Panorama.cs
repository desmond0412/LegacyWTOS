using UnityEngine;
using System.Collections;

namespace Artoncode.Core.UI
{
	public class Panorama : MonoBehaviour
	{
		public delegate void PanoramaDelegate (GameObject sender);

		public event PanoramaDelegate OnFadeInStart;
		public event PanoramaDelegate OnFadeInEnd;
		public event PanoramaDelegate OnFadeOutStart;
		public event PanoramaDelegate OnFadeOutEnd;


		public float fadeTime = 1;
		public float fadeSpeed = 2;
		public Color fadeColor = Color.black;

		private Material m_Material = null;
		private bool m_FadingIn = false;
		private bool m_FadingOut = false;


		private void Awake ()
		{
#pragma warning disable 618
			m_Material = new Material ("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
#pragma warning restore 618
		}
	
		private void DrawQuad (Color aColor, float aAlpha)
		{
			aColor.a = aAlpha;
			m_Material.SetPass (0);
			GL.Color (aColor);
			GL.PushMatrix ();
			GL.LoadOrtho ();
			GL.Begin (GL.QUADS);
			GL.Vertex3 (0, 0.85f, -1);
			GL.Vertex3 (0, 1.0f, -1);
			GL.Vertex3 (1, 1.0f, -1);
			GL.Vertex3 (1, 0.85f, -1);
			GL.End ();

			GL.Begin (GL.QUADS);
			GL.Vertex3 (0, 0.0f	, -1);
			GL.Vertex3 (0, 0.15f, -1);
			GL.Vertex3 (1, 0.15f, -1);
			GL.Vertex3 (1, 0.0f	, -1);
			GL.End ();

			GL.PopMatrix ();
		}


		private IEnumerator FadeCoroutine (float fadeDuration, Color aColor)
		{

			//FADE OUT AND FADE IN  in 1 time
			float t = 0.0f;
			if (OnFadeOutStart != null)
				OnFadeOutStart (this.gameObject);		
			while (t<1.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t + Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeOutEnd != null)
				OnFadeOutEnd (this.gameObject);		

			float time = 0;
			while(m_FadingOut && time < fadeDuration)
			{
				yield return new WaitForEndOfFrame ();
				DrawQuad (aColor, t);
				time+= Time.deltaTime;
			}


			if (OnFadeInStart != null)
				OnFadeInStart (this.gameObject);		
			while (t>0.0f) {
				yield return new WaitForEndOfFrame ();
				t = Mathf.Clamp01 (t - Time.deltaTime * fadeSpeed);
				DrawQuad (aColor, t);
			}
			if (OnFadeInEnd != null)
				OnFadeInEnd (this.gameObject);		

			m_FadingOut = false;
			m_FadingIn = false;
		}

		public void PanoramicStart( float aFadeDuration =-1 )
		{
			if (m_FadingIn) return;
			if (m_FadingOut) return;
			m_FadingOut = true;
			m_FadingIn = true;

			if (aFadeDuration == -1)
				aFadeDuration = 999;
			StartCoroutine (FadeCoroutine (aFadeDuration, Color.black));
		}

		public void PanoramicStop()
		{
			m_FadingOut = false;
			m_FadingIn = false;
		}

	}
}
