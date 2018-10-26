// 
// CharacterMoveTo.cs
// 
// Copyright (c) 2011-2015, Candlelight Interactive, LLC
// All rights reserved.
// 
// This file is licensed according to the terms of the Unity Asset Store EULA:
// http://download.unity3d.com/assetstore/customer-eula.pdf

using UnityEngine;

namespace Candlelight.Examples.CustomHandles
{
	/// <summary>
	/// A component that works with a <see cref="UnityEngine.CharacterController"/> to move a character toward a target
	/// location.
	/// </summary>
	[AddComponentMenu("Characters/Character Move To")]
	[RequireComponent(typeof(CharacterController))]
	public class CharacterMoveTo : MonoBehaviour
	{
		#region Shared Allocations
		private RaycastHit s_Hit;
		#endregion

		/// <summary>
		/// The transform component.
		/// </summary>
		private Transform m_Transform;

		#region Backing Fields
		[SerializeField, HideInInspector]
		private CharacterController m_Character;
		[SerializeField]
		private LayerMask m_ClickableSurfaces;
		private Vector3 m_DesiredPosition;
		[SerializeField]
		private bool m_IsHandlingInput = true;
		[SerializeField]
		private float m_MovementPrecision = 0.05f;
		[SerializeField]
		private AnimationCurve m_SpeedCurve = AnimationCurve.EaseInOut(2f, 2f, 5f, 4f);
		[SerializeField]
		private float m_TurnDamping = 10f;
		[SerializeField]
		private LayerMask m_WalkableSurfaces;
		#endregion

		/// <summary>
		/// Gets the character controller.
		/// </summary>
		/// <value>The character controller.</value>
		public CharacterController Character
		{
			get { return m_Character = m_Character == null ? GetComponent<CharacterController>() : m_Character; }
		}
		/// <summary>
		/// Gets or sets the clickable surfaces.
		/// </summary>
		/// <value>The clickable surfaces.</value>
		public LayerMask ClickableSurfaces
		{
			get { return m_ClickableSurfaces; }
			set { m_ClickableSurfaces = value; }
		}
		/// <summary>
		/// Gets or sets the desired position.
		/// </summary>
		/// <value>The desired position.</value>
		public Vector3 DesiredPosition
		{
			get { return m_DesiredPosition; }
			set { SetDesiredPosition(value); }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Candlelight.CharacterMoveTo"/> is handling input.
		/// </summary>
		/// <value><see langword="true"/> if is handling input; otherwise, <see langword="false"/>.</value>
		public bool IsHandlingInput
		{
			get { return m_IsHandlingInput; }
			set { m_IsHandlingInput = value; }
		}
		/// <summary>
		/// Gets a value indicating whether this <see cref="CharacterMoveTo"/> is moving.
		/// </summary>
		/// <value><see langword="true"/> if is moving; otherwise, <see langword="false"/>.</value>
		public bool IsMoving { get { return (Character == null) ? false : Character.velocity.sqrMagnitude > 0f; } }
		/// <summary>
		/// Gets or sets the distance inside which the character is presumed to be at its desired position.
		/// </summary>
		/// <value>The movement precision.</value>
		public float MovementPrecision
		{
			get { return m_MovementPrecision; }
			set { m_MovementPrecision = value; }
		}
		/// <summary>
		/// Gets the run animation amount for blending.
		/// </summary>
		/// <value>The run animation amount.</value>
		public float RunAnimationAmount { get { return !IsMoving ? 0f : Speed / RunSpeed; } }
		/// <summary>
		/// Gets the distance at which the character uses run speed.
		/// </summary>
		/// <value>The distance at which the character uses run speed.</value>
		public float RunDistance { get { return SpeedCurve.keys[SpeedCurve.length-1].time; } }
		/// <summary>
		/// Gets the run speed.
		/// </summary>
		/// <value>The run speed.</value>
		public float RunSpeed { get { return SpeedCurve.keys[SpeedCurve.length-1].value; } }
		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		/// <value>The speed.</value>
		public float Speed { get; private set; }
		/// <summary>
		/// Gets or sets curve specifying the character's run and walk speeds, and the distances at which each is used.
		/// </summary>
		/// <value>The speed curve.</value>
		public AnimationCurve SpeedCurve
		{
			get { return m_SpeedCurve; }
			set { m_SpeedCurve = value; }
		}
		/// <summary>
		/// Gets or sets the damping value on the character's turning speed (higher value is a faster turn).
		/// </summary>
		/// <value>The turn damping.</value>
		public float TurnDamping
		{
			get { return m_TurnDamping; }
			set { m_TurnDamping = value; }
		}
		/// <summary>
		/// Gets or sets the surfaces where a click will actually count to set target position.
		/// </summary>
		/// <value>The walkable surfaces.</value>
		public LayerMask WalkableSurfaces
		{
			get { return m_WalkableSurfaces; }
			set { m_WalkableSurfaces = value; }
		}
		/// <summary>
		/// Gets the velocity.
		/// </summary>
		/// <value>The velocity.</value>
		public Vector3 Velocity { get { return Speed * (DesiredPosition - m_Transform.position); } }
		/// <summary>
		/// Gets the walk animation amount for blending.
		/// </summary>
		/// <value>The walk animation amount.</value>
		public float WalkAnimationAmount { get { return !IsMoving ? 0f : 1f - RunAnimationAmount; } }
		/// <summary>
		/// Gets the distance at which the character uses walk speed.
		/// </summary>
		/// <value>The distance at which the character uses walk speed.</value>
		public float WalkDistance { get { return SpeedCurve.keys[0].time; } }
		/// <summary>
		/// Gets the walk speed.
		/// </summary>
		/// <value>The walk speed.</value>
		public float WalkSpeed { get { return SpeedCurve.keys[0].value; } }
		
		/// <summary>
		/// Send an input position (e.g., mouse click or touch) to move to.
		/// </summary>
		/// <param name='v'>Screen position.</param>
		public void InputPosition(Vector3 v)
		{
			if (
				!UnityEngine.Physics.Raycast(
					Camera.main.ScreenPointToRay(v), out s_Hit, Mathf.Infinity, m_ClickableSurfaces
				)
			)
			{
				return;
			}
			if ((m_WalkableSurfaces & 1 << s_Hit.collider.gameObject.layer) == 0)
			{
				return;
			}
			SetDesiredPosition(s_Hit.point);
		}
		
		/// <summary>
		/// Move the character to the specified position.
		/// </summary>
		/// <param name='v'>Position.</param>
		private void MoveTo(Vector3 v)
		{
			// early out if the character controller no longer exists
			if (this.Character == null)
			{
				return;
			}
			// get v relative to the character
			v = v - m_Transform.position;
			// zero out the y-component of the incoming movement vector since the character only moves in a plane
			v.y = 0f;
			// set the speed from the movement curve based on the distance to the target position
			float mag = v.magnitude;
			float movementSpeed = m_SpeedCurve.Evaluate(mag);
			// only move the character if it is outside the movement precision threshold
			if (mag > m_MovementPrecision)
			{
				// point the character in the direction it should be facing
				TurnTo(v);
				this.Speed = movementSpeed;
			}
			else
			{
				this.Speed = 0f;
				v = Vector3.zero;
			}
			// move the character!
			Character.SimpleMove(v.normalized * movementSpeed);
		}
		
		/// <summary>
		/// Update the desired position.
		/// </summary>
		/// <remarks>
		/// Used to be an RPC, so leaving it as-is.
		/// </remarks>
		/// <param name='p'>Position.</param>
		private void SetDesiredPosition(Vector3 p)
		{
			p.y = m_Transform.position.y;
			m_DesiredPosition = p;
		}
		
		/// <summary>
		/// Initialize.
		/// </summary>
		void Start()
		{
			m_Transform = Character.transform;
			this.DesiredPosition = m_Transform.position;
		}
		
		/// <summary>
		/// Turn to face in the specified direction.
		/// </summary>
		/// <param name='direction'>Direction.</param>
		private void TurnTo(Vector3 direction)
		{
			// limit rotation to y-axis
			direction.y = 0;
			// use the dampening value to interpolate rotation from the current orientation to the desired heading
			transform.rotation *= Quaternion.Slerp(
				Quaternion.identity,
				Quaternion.FromToRotation(m_Transform.forward, direction),
				m_TurnDamping * Time.deltaTime
			);
		}
		
		/// <summary>
		/// Move toward the desired position.
		/// </summary>
		void Update()
		{
			// process input if requested
			if (m_IsHandlingInput)
			{
				if (Input.GetMouseButtonDown(0))
				{
					InputPosition(Input.mousePosition);
				}
				foreach (Touch t in Input.touches)
				{
					if (t.phase!=TouchPhase.Moved && t.phase!=TouchPhase.Stationary)
					{
						InputPosition(t.position);
					}
				}
			}
			// keep on truckin'
			MoveTo(this.DesiredPosition);
		}
	}
}