using UnityEngine;
using System.Collections;
using System.IO;

public class DrawToFBO : MonoBehaviour {
	RenderTexture drawTexture;
	public Material transparentMat;
	public Color initialClear;
	public GameObject brush;
	Camera thisCam;
	Texture2D saveTex;
	bool needScreenShot = false;
	// Use this for initialization
	void Start () {
		thisCam = GetComponent<Camera> ();
		drawTexture = new RenderTexture (200, 200, 16, RenderTextureFormat.ARGB32);
		drawTexture.filterMode = FilterMode.Point;
		RenderTexture.active = drawTexture;
		GL.Clear (true, true, initialClear);
	}
	public RenderTexture GetDrawTexture(){
		return drawTexture;
	}
	public void ClearDrawTexture(){
		RenderTexture.active = drawTexture;
		GL.Clear (true, true, initialClear);
		RenderTexture.active = null;
	}
	void OnPreRender(){
		RenderTexture.active = drawTexture;
	}
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(drawTexture, dest, transparentMat);
		RenderTexture.active = null;
	}
	void OnPostRender(){
	}
}
