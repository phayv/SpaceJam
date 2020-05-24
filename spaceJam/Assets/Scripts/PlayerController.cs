using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private Rigidbody2D _rigidBody;
	[SerializeField] private BoxCollider2D _boxCollider;
	private Animator _animator;

	public Movement IntMovementNumber
	{
		get { return (Movement) _animator.GetInteger(MovementNumber); }
		set
		{
			_animator.SetInteger(MovementNumber, (int) value);
		}
	}

	[Header("Values")] 
	[SerializeField] private float _force = 0f;

	private static readonly int MovementNumber = Animator.StringToHash("MovementNumber");

	private void Awake()
	{
		_animator= GetComponent<Animator>();
	}

	public enum Movement
	{
		None = 0, 
		Left = 1, 
		Right = 2, 
		Jump = 3
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.D))
		{
			Evt_Movement(Movement.Right);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			Evt_Movement(Movement.Left);
		}
		else
		{
			Evt_Movement(Movement.None);
		}
	}
	
	public void Evt_Movement(Movement movement)
	{
		var force = 0f;
		IntMovementNumber = movement;
		switch (movement)
		{
			case Movement.Right:
				_rigidBody.AddForce(new Vector2(_force, 0), ForceMode2D.Impulse);
				break;
			case Movement.Left:
				_rigidBody.AddForce(new Vector2(-1 * _force,0), ForceMode2D.Impulse);
				break;
			case Movement.Jump:
				break;
		}
	}
}
