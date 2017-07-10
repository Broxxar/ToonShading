using UnityEngine;

/// <summary>
/// A Fake3DSprite.
/// </summary>
public class Fake3DSprite : MonoBehaviour
{
	[SerializeField]
	private GameObject _prefab;

	[SerializeField]
	private Renderer _renderer;

	private Fake3DSpriteRenderer _sprite3DRenderer;

	public GameObject Sprite3D { get; private set; }

	/// <summary>
	/// Instantiates the 3D character and Fake3DSpriteRenderer.
	/// 
	/// Then sets the renderer assigned in the inspector's main material texture
	/// to the render texture created by the Fake3DSpriteRenderer.
	/// </summary>
	public void Initalize()
	{
		if (_prefab != null)
		{
			Sprite3D = Instantiate(_prefab);
			_sprite3DRenderer = Instantiate(Resources.Load<Fake3DSpriteRenderer>("Fake3DSpriteRenderer"));
			_sprite3DRenderer.Initalize(Sprite3D);
			_renderer.material.SetTexture("_MainTex", _sprite3DRenderer.Texture);
		}
	}
}
