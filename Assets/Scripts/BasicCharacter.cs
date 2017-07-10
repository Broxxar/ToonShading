using UnityEngine;

/// <summary>
/// Manages character setup and runtime movement.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
	[SerializeField]
	private Fake3DSprite _fakeSprite;

	private GameObject _character;
	private Animator _characterAnim;

	private readonly float _moveSpeedMax = 3.0f;
	private readonly float _moveSpeedSmoothTime = 0.075f;
	private Vector2 _moveSpeedCurrent;
	private Vector2 _moveSpeedTarget;
	private Vector2 _moveSpeedVelocity;

	/// <summary>
	/// Initializes the off screen character on start and grabs it's Animator.
	/// Also tells all the Grass in the scene to track it (so they can fade out when the player is close).
	/// </summary>
	private void Awake()
	{
		_fakeSprite.Initalize();
		_character = _fakeSprite.Sprite3D;
		_characterAnim = _character.GetComponent<Animator>();

		var grassFaders = Resources.FindObjectsOfTypeAll<GrassFader>();
		foreach (var fader in grassFaders)
		{
			fader.TrackedObject = transform;
		}
	}

	/// <summary>
	/// Basic player movement.
	/// </summary>
	private void Update()
	{
		var frameMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		_moveSpeedTarget = frameMovement.normalized * _moveSpeedMax;
		_moveSpeedCurrent = Vector2.SmoothDamp(_moveSpeedCurrent, _moveSpeedTarget, ref _moveSpeedVelocity, _moveSpeedSmoothTime, float.PositiveInfinity, Time.deltaTime);
		transform.Translate(_moveSpeedCurrent * Time.deltaTime);
		_characterAnim.SetFloat("CurrentMoveSpeed", _moveSpeedCurrent.magnitude);

		if (frameMovement != Vector2.zero)
		{
			_character.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-_moveSpeedCurrent.y, _moveSpeedCurrent.x) * Mathf.Rad2Deg - 90.0f, 0);
		}
	}
}
