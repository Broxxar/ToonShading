using UnityEngine;

/// <summary>
/// Simple Camera movement script.
/// </summary>
public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	private readonly float _smoothTime = 0.250f;
	private readonly float _zOffset = -10.0f;

	private Vector3 _targetPostion;
	private Vector3 _velocity;

	/// <summary>
	/// Smoothly follow the player.
	/// </summary>
	private void LateUpdate()
	{
		if (_target != null)
		{
			var targetPos = new Vector3(_target.position.x, _target.position.y, _zOffset);
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _smoothTime);
		}
	}
}
