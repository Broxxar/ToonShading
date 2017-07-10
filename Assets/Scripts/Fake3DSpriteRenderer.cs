using UnityEngine;

/// <summary>
/// Manages the offscreen render of a gameobject.
/// </summary>
public class Fake3DSpriteRenderer : MonoBehaviour
{
	public RenderTexture Texture { get; private set; }

	[SerializeField]
	private Camera _camera;

	/// <summary>
	/// Creates a new render texture for the camera (set in inspector) to render.
	/// This camera should have cull flags and be placed such that it only sees one Object.
	/// 
	/// A manager on top of this class that handles multiple off screen cameras would be ideal,
	/// but in the most basic case you just need to move the game object's apart from eachother.
	/// </summary>
	public void Initalize(GameObject targetObject)
	{
		Texture = new RenderTexture(256, 256, 24)
		{
			name = targetObject.name + " Renderer",
			filterMode = FilterMode.Bilinear,
			antiAliasing = QualitySettings.antiAliasing > 0 ? QualitySettings.antiAliasing : 1
		};

		_camera.targetTexture = Texture;
        targetObject.transform.SetParent(transform);
    }
}