using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Quick and dirty class to fade out the grass as the player approaches.
/// </summary>
public class GrassFader : MonoBehaviour
{
	[System.NonSerialized]
	public Transform TrackedObject;

	private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

	private void Awake()
	{
		GetComponentsInChildren(_sprites);
	}

	private void Update()
	{
		if (TrackedObject != null)
		{
			foreach (var sprite in _sprites)
			{
				var distance = Vector3.Distance(transform.position, TrackedObject.position);
				var alpha = Mathf.Clamp01(Remap(0.5f, 2.0f, 0.35f, 1.0f, distance));
				sprite.color = new Color(1, 1, 1, alpha);
			}
		}
	}

	private float Remap(float l0, float h0, float l1, float h1, float t)
	{
		return (l1 + (t - l0) * (h1 - l1)) / (h0 - l0);
	}
}
